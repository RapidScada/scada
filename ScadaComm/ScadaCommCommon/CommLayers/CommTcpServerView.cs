/*
 * Библиотека слоёв связи
 * Пользовательский интерфейс слоя связи TCP-сервер
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using System.Collections.Generic;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// Пользовательский интерфейс слоя связи TCP-сервер
    /// </summary>
    public class CommTcpServerView : CommLayerView
    {
        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ? "TCP сервер" : "TCP server";
            }
        }

        /// <summary>
        /// Получить описание слоя связи
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Слой связи TCP сервер.\n\n" +
                    "Параметры слоя связи:\n" +
                    "TcpPort - TCP-порт для входящих соединений,\n" +
                    "InactiveTime - время неактивности соединения до отключения, с,\n" +
                    "Behavior - режим работы слоя связи (Master, Slave),\n" +
                    "DevSelMode - режим выбора КП (ByIPAddress, ByFirstPackage, ByDeviceLibrary)." :

                    "TCP server communication layer.\n\n" +
                    "Communication layer parameters:\n" +
                    "TcpPort - TCP port for incoming connections." +
                    "InactiveTime - duration of inactivity before disconnect, sec,\n" +
                    "Behavior - work mode of connection layer (Master, Slave),\n" +
                    "DevSelMode - device selection mode (ByIPAddress, ByFirstPackage, ByDeviceLibrary).";
            }
        }

        /// <summary>
        /// Получить информацию о свойствах слоя связи
        /// </summary>
        public override string GetPropsInfo(Dictionary<string, string> layerParams)
        {
            CommTcpServerLogic.Settings defSett = new CommTcpServerLogic.Settings();
            return BuildPropsInfo(layerParams, 
                new string[] { "TcpPort", "InactiveTime", "Behavior", "DevSelMode" },
                new object[] { defSett.TcpPort, defSett.InactiveTime, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
