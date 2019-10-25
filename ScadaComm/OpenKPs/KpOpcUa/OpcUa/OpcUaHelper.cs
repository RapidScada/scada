/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Helper methods to work using OPC UA
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Scada.Comm.Devices.OpcUa.Config;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Scada.Comm.Devices.OpcUa
{
    /// <summary>
    /// Helper methods to work using OPC UA.
    /// <para>Вспомогательные методы для работы с OPC UA.</para>
    /// </summary>
    public class OpcUaHelper
    {
        /// <summary>
        /// The runtime kinds.
        /// </summary>
        public enum RuntimeKind { Logic, View };

        /// <summary>
        /// The OPC UA configuration file name for the logic runtime.
        /// </summary>
        private const string LogicOpcConfig = "KpOpcUa.Logic.xml";
        /// <summary>
        /// The OPC UA configuration file name for the view runtime.
        /// </summary>
        private const string ViewOpcConfig = "KpOpcUa.View.xml";

        private readonly AppDirs appDirs;     // the application directories
        private readonly string kpNumStr;     // the device number as a string
        private readonly RuntimeKind runtime; // the runtime kind


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcUaHelper(AppDirs appDirs, int kpNum, RuntimeKind runtime)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            kpNumStr = CommUtils.AddZeros(kpNum, 3);
            this.runtime = runtime;

            AutoAccept = false;
            OpcSession = null;
            CertificateValidation = null;
            WriteToLog = (string text) => { }; // stub
        }


        /// <summary>
        /// Gets or sets the certificate validation method.
        /// </summary>
        public CertificateValidationEventHandler CertificateValidation { get; set; }

        /// <summary>
        /// Gets or sets the logging method.
        /// </summary>
        public Log.WriteLineDelegate WriteToLog { get; set; }

        /// <summary>
        /// Gets a value indicating whether to automatically accept server certificates.
        /// </summary>
        public bool AutoAccept { get; private set; }

        /// <summary>
        /// Gets the OPC session
        /// </summary>
        public Session OpcSession { get; private set; }


        /// <summary>
        /// Writes an OPC UA configuration file depending on operating system and runtime kind.
        /// </summary>
        private void WriteConfigFile(out string fileName)
        {
            fileName = Path.Combine(appDirs.ConfigDir, runtime == RuntimeKind.View ? ViewOpcConfig : LogicOpcConfig);

            if (!File.Exists(fileName))
            {
                string resourceName = ScadaUtils.IsRunningOnWin ?
                    "Scada.Comm.Devices.Config.KpOpcUa.Win.xml" :
                    "Scada.Comm.Devices.Config.KpOpcUa.Linux.xml";
                string fileContents;

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                }

                File.WriteAllText(fileName, fileContents, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Connects to the OPC server asynchronously.
        /// </summary>
        public async Task<bool> ConnectAsync(ConnectionOptions connectionOptions, int operationTimeout = -1)
        {
            AutoAccept = false;
            OpcSession = null;

            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = string.Format("KpOpcUa_{0} Driver", kpNumStr),
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Scada.Comm.Devices.KpOpcUa"
            };

            // load the application configuration
            WriteConfigFile(out string configFileName);
            ApplicationConfiguration config = await application.LoadApplicationConfiguration(configFileName, false);

            // check the application certificate
            bool haveAppCertificate = await application.CheckApplicationInstanceCertificate(false, 0);

            if (!haveAppCertificate)
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Сертификат экземпляра приложения недействителен!" :
                    "Application instance certificate invalid!");
            }

            if (haveAppCertificate)
            {
                config.ApplicationUri = Opc.Ua.Utils.GetApplicationUriFromCertificate(
                    config.SecurityConfiguration.ApplicationCertificate.Certificate);

                if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                {
                    AutoAccept = true;
                }

                if (CertificateValidation != null)
                    config.CertificateValidator.CertificateValidation += CertificateValidation;
            }
            else
            {
                WriteToLog(Localization.UseRussian ?
                    "Предупреждение: отсутствует сертификат приложения, используется незащищенное соединение." :
                    "Warning: missing application certificate, using unsecure connection.");
            }

            // create session
            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(
                connectionOptions.ServerUrl, haveAppCertificate, operationTimeout);
            selectedEndpoint.SecurityMode = connectionOptions.SecurityMode;
            selectedEndpoint.SecurityPolicyUri = connectionOptions.GetSecurityPolicy();
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
            UserIdentity userIdentity = connectionOptions.AuthenticationMode == AuthenticationMode.Username ?
                new UserIdentity(connectionOptions.Username, connectionOptions.Password) :
                new UserIdentity(new AnonymousIdentityToken());

            OpcSession = await Session.Create(config, endpoint, false,
                "Rapid SCADA KpOpcUa_" + kpNumStr,
                (uint)config.ClientConfiguration.DefaultSessionTimeout, userIdentity, null);

            WriteToLog(string.Format(Localization.UseRussian ?
                "OPC сессия успешно создана: {0}" :
                "OPC session created successfully: {0}", connectionOptions.ServerUrl));
            return true;
        }
    }
}
