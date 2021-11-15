/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Represents a control for editing archive upload option
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Scada.Server.Modules.DbExport.Config;
using Scada.UI;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Represents a control for editing archive upload options.
    /// <para>Представляет элемент управления для редактирования параметров загрузки архивов.</para>
    /// </summary>
    public partial class CtrlArcUploadOptions : UserControl
    {
        private ArcUploadOptions arcUploadOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlArcUploadOptions()
        {
            InitializeComponent();
            cbSnapshotType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSnapshotType.Items.Clear();
            cbSnapshotType.Items.Add(SnapshotType.Cur);
            cbSnapshotType.Items.Add(SnapshotType.Min);
            cbSnapshotType.Items.Add(SnapshotType.Hour);
            arcUploadOptions = null;
        }


        /// <summary>
        /// Gets or sets an editable archive upload options.
        /// </summary>
        internal ArcUploadOptions ArcUploadOptions
        {
            get
            {
                return arcUploadOptions;
            }
            set
            {
                if (value != null)
                {
                    arcUploadOptions = null;

                    if (value.SnapshotType == SnapshotType.Cur)
                    {
                        cbSnapshotType.SelectedIndex = 0;
                    }
                    else
                    {
                        if (value.SnapshotType == SnapshotType.Min)
                            cbSnapshotType.SelectedIndex = 1;
                        else
                            cbSnapshotType.SelectedIndex = 2;
                    }

                    numDelay.Value = value.Delay;
                    numMaxAge.Value = value.MaxAge;
                    chkEnabled.Checked = value.Enabled;
                }

                arcUploadOptions = value;
            }
        }

        /// <summary>
        /// Trigges an event ArcUploadOptionsChanged.
        /// </summary>
        private void OnArcUploadOptionsChanged()
        {
            ArcUploadOptionsChanged?.Invoke(this, new ObjectChangedEventArgs(arcUploadOptions));
        }

        /// <summary>
        /// An event that occurs when the properties of an edited archive upload options.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ArcUploadOptionsChanged;


        private void cbSnapshotType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arcUploadOptions != null)
            {
                arcUploadOptions.SnapshotType = (SnapshotType)cbSnapshotType.SelectedIndex;
                OnArcUploadOptionsChanged();
            }
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            if (arcUploadOptions != null)
            {
                arcUploadOptions.Delay = Convert.ToInt32(numDelay.Value);
                OnArcUploadOptionsChanged();
            }
        }

        private void numMaxAge_ValueChanged(object sender, EventArgs e)
        {
            if (arcUploadOptions != null)
            {
                arcUploadOptions.MaxAge = Convert.ToInt32(numMaxAge.Value);
                OnArcUploadOptionsChanged();
            }
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (arcUploadOptions != null)
            {
                arcUploadOptions.Enabled = chkEnabled.Checked;
                OnArcUploadOptionsChanged();
            }
        }
    }
}
