/*
 * Copyright 2015 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ModDBExport
 * Summary  : Module configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Module configuration form
    /// <para>Форма конфигурации КП</para>
    /// </summary>
    public partial class FrmDBExportConfig : Form
    {
        private string configDir;  // директория конфигурации
        private string langDir;    // директория языковых файлов
        private string logDir;     // директория журналов

        private Config config;     // конфигурация модуля
        private Config configCopy; // копия конфигурации модуля для реализации отмены изменений
        private bool modified;     // признак изменения конфигурации


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDBExportConfig()
        {
            InitializeComponent();

            config = null;
            configCopy = null;
            modified = false;
        }


        /// <summary>
        /// Получить или установить признак изменения конфигурации
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
                btnCancel.Enabled = modified;
            }
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(string configDir, string langDir, string logDir)
        {
            FrmDBExportConfig frmDBExportConfig = new FrmDBExportConfig();
            frmDBExportConfig.configDir = configDir;
            frmDBExportConfig.langDir = langDir;
            frmDBExportConfig.logDir = logDir;
            frmDBExportConfig.ShowDialog();
        }

        /// <summary>
        /// Отобразить конфигурацию
        /// </summary>
        private void ConfigToControls()
        {
        }


        private void FrmDBExportConfig_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(langDir, "ModDBExport", out errMsg))
                {
                    Localization.TranslateForm(this, "Scada.Server.Modules.FrmDBExportConfig");
                    //ModPhrases.Init();
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }

            // загрузка конфигурации
            config = new Config(configDir);
            if (File.Exists(config.FileName) && !config.Load(out errMsg))
                ScadaUtils.ShowError(errMsg);

            // создание копии конфигурации
            configCopy = config.Clone();

            // отображение конфигурации
            ConfigToControls();

            // снятие признака изменения конфигурации
            Modified = false;
        }
    }
}
