using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    public partial class FrmDBExportConfig : Form
    {
        private string configDir;          // директория конфигурации
        private string logDir;             // директория журналов


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDBExportConfig()
        {
            InitializeComponent();

            configDir = "";
            logDir = "";
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(string configDir, string logDir)
        {
            FrmDBExportConfig frmDBExportConfig = new FrmDBExportConfig();
            frmDBExportConfig.configDir = configDir;
            frmDBExportConfig.logDir = logDir;
            frmDBExportConfig.ShowDialog();
        }
    }
}
