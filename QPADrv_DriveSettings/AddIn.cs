// ============================
// AddIn.cs Update
// ============================

using Siemens.Engineering;
using Siemens.Engineering.AddIn.Menu;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using WPF;
using System.Windows.Forms;

namespace QPADrv_DriveSettings
{
    public class AddIn : ContextMenuAddIn
    {
        private TiaPortal _tiaportal;

        public AddIn(TiaPortal tiaportal) : base("QPADrv_DriveSettings")
        {
            _tiaportal = tiaportal;
        }

        /// <summary>
        /// Context Menu creation for Export and Start Add-In.
        /// </summary>
        protected override void BuildContextMenuItems(ContextMenuAddInRoot addInRootSubmenu)
        {
            addInRootSubmenu.Items.AddActionItem<DeviceItem>("Start Add-In", StartAddIn_Click, OnCanExecute);
        }

        private void StartAddIn_Click(MenuSelectionProvider<DeviceItem> menuSelectionProvider)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        private MenuStatus OnCanExecute(MenuSelectionProvider<DeviceItem> menuSelectionProvider)
        {
            return MenuStatus.Enabled;
        }
    }
}
