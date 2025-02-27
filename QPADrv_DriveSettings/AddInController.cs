using Siemens.Engineering;
using Siemens.Engineering.AddIn.Menu;
using Siemens.Engineering.HW;
using Siemens.Engineering.MC.Drives;
using System;
using System.Collections.Generic;

using static SDRhelper.StartdriveHelper;
using WPF;
using System.Windows.Controls;
using System.Windows.Forms;

namespace QPADrv_DriveSettings
{
    public class AddInController : ContextMenuAddIn
    {
        /// <summary>
        ///The global TIA Portal Object 
        ///<para>It will be used in the TIA Add-In.</para>
        /// </summary>
        TiaPortal _tiaportal;
        String _logText;
        private LogForm logForm;

        /// <summary>
        /// The display name of the Add-In.
        /// </summary>
        private const string s_DisplayNameOfAddIn = "QPADrv_DriveSettings";

        /// <summary>
        /// The constructor of the AddIn.
        /// Creates an object of the class AddIn
        /// Called from AddInProvider, when the first
        /// right-click is performed in TIA
        /// Motherclass' constructor of ContextMenuAddin
        /// will be executed, too. 
        /// <param name="tiaportal">
        /// Represents the actual used TIA Portal process.
        /// </param>
        /// </summary>
        public AddInController(TiaPortal tiaportal) : base(s_DisplayNameOfAddIn)
        {
            /*
             * The acutal TIA Portal process is saved in the 
             * global TIA Portal variable _tiaportal
             * tiaportal comes as input Parameter from the
             * AddInProvider
            */
            _tiaportal = tiaportal;
            logForm = new LogForm();
            WriteLog("Addin started");
        }

        public void ShowLogForm()
        {
            if (logForm.IsDisposed)
            {
                logForm = new LogForm();
                WriteLog("Yeniden olusturuldu");
            }
            logForm.Show();
            logForm.BringToFront();
            WriteLog("Logform gosterildi");
        }

        public void WriteLog(string message)
        {
            try
            {
                if (logForm.InvokeRequired)
                {
                    logForm.Invoke(new Action(() => logForm.AppendLog(message)));
                }
                else
                {
                    logForm.AppendLog(message);
                }
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Log form kapatıldıktan sonra log yazılamaz. Lütfen formu tekrar açın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Log yazılırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// The method is supplemented to include the Add-In
        /// in the Context Menu of TIA Portal.
        /// Called when a right-click is performed in TIA
        /// and a mouse-over is performed on the name of the Add-In.
        /// <typeparam name="addInRootSubmenu">
        /// The Add-In will be displayed in 
        /// the Context Menu of TIA Portal.
        /// </typeparam>
        /// <example>
        /// ActionItems like Buttons/Checkboxes/Radiobuttons
        /// are possible. In this example, only Buttons will be created 
        /// which will start the Add-In program code.
        /// </example>
        /// </summary>
        protected override void BuildContextMenuItems(ContextMenuAddInRoot
            addInRootSubmenu)
        {
            /* Method addInRootSubmenu.Items.AddActionItem
             * Will Create a Pushbutton with the text 'Start Add-In Code'
             * 1st input parameter of AddActionItem is the text of the 
             *          button
             * 2nd input parameter of AddActionItem is the clickDelegate, 
             *          which will be executed in case the button 'Start 
             *          Add-In Code' will be clicked/pressed.
             * 3rd input parameter of AddActionItem is the 
             *          updateStatusDelegate, which will be executed in 
             *          case there is a mouseover the button 'Start 
             *          Add-In Code'.
             * in <placeholder> the type of AddActionItem will be 
             *          specified, because AddActionItem is generic 
             * AddActionItem<DeviceItem> will create a button that will be 
             *          displayed if a rightclick on a DeviceItem will be 
             *          performed in TIA Portal
             * AddActionItem<Project> will create a button that will be 
             *          displayed if a rightclick on the project name 
             *          will be performed in TIA Portal
            */

            addInRootSubmenu.Items.AddActionItem<DeviceItem>(
                "Export", OnExportSelected,
                OnCanSomething);
            addInRootSubmenu.Items.AddActionItem<DeviceItem>(
                "Start Add-In", OnStartAddInSelected,
                OnCanSomething);
            addInRootSubmenu.Items.AddActionItem<Project>(
                "Not Available here", OnClickProject,
                OnStatusUpdateProject);
        }
        private void OnExportSelected(MenuSelectionProvider<DeviceItem> menuSelectionProvider)
        {
            IEnumerable<DeviceItem> selection = menuSelectionProvider.GetSelection<DeviceItem>();
            //foreach (DeviceItem actDeviceItem in selection)
            //{
            //    MainWindow myWindow = new MainWindow(actDeviceItem.Name);
            //    myWindow.OpenExportPage(actDeviceItem.Name);  // Uses MainWindow's method
            //    myWindow.ShowDialog(); // Shows the window with ExportPage loaded in the frame
            //}
        }


        private void OnStartAddInSelected(MenuSelectionProvider<DeviceItem> menuSelectionProvider)
        {
            IEnumerable<DeviceItem> selection = menuSelectionProvider.GetSelection<DeviceItem>();
            //foreach (DeviceItem actDeviceItem in selection)
            //{
            //    MainWindow myWindow = new MainWindow(actDeviceItem.Name);
            //    myWindow.NavigateToDriveList(actDeviceItem.Name, "Started");
            //    myWindow.ShowDialog(); // Open new window
            //}
        }

        /// <summary>
        /// The method contains the program code of the TIA Add-In.
        /// Called when the button 'Start Add-In Code' will be pressed.
        /// <para>MenuSelectionProvider DeviceItem menuSelectionProvider
        /// here, the same type as in addInRootSubmenu.Items.AddActionItem
        /// must be used -> here it is DeviceItem
        /// </para>
        /// </summary>
        private void OnDoSomething(MenuSelectionProvider<DeviceItem>
            menuSelectionProvider)
        {


            //ShowLogForm();
            //logForm.BringToFront();
            DriveObject myDriveObject = null;

            //Get the actual selected DeviceItem from TIA Portal
            IEnumerable<DeviceItem> selection =
                menuSelectionProvider.GetSelection<DeviceItem>();

            //change parameters for each selected drive in TIA Portal
            foreach (DeviceItem actDeviceItem in selection)
            {

                //MainWindow exportWindow = new MainWindow(actDeviceItem.Name);
                //exportWindow.OpenExportPage(actDeviceItem.Name);
                //exportWindow.ShowDialog();

                //MainWindow driveListWindow = new MainWindow(_logText);
                //driveListWindow.ShowDialog();

                /*
                 * get the SINAMICS DriveObject 
                 * S120, S120 Integrated, G120, G115D, G110M
                 */
                try
                {
                    myDriveObject =
                    actDeviceItem.GetService<DriveObjectContainer>().
                    DriveObjects[0];
                }
                /*
                 * get the SINAMICS DriveObject 
                 * S210
                 */
                catch
                {
                    Device drive_unit =
                        (Device)actDeviceItem.Parent;

                    if (drive_unit.TypeIdentifier.ToString().
                        Contains("S210"))
                    {
                        foreach (DeviceItem deviceItems
                            in drive_unit.DeviceItems)
                        {
                            if (deviceItems.Classification ==
                                DeviceItemClassifications.None)
                            {
                                myDriveObject =
                                deviceItems.GetService<DriveObjectContainer>().
                                DriveObjects[0];
                            }
                        }
                    }
                    //no SINAMICS drive found
                    else
                    {
                        myDriveObject = null;
                    }

                }
                //Start Code to adjust SINAMICS drive parameters 
                if (myDriveObject != null)
                {
                    AdjustParameters(myDriveObject);
                    //WriteLog("test1");
                }
            }
            //DeviceItem test =
            //       (DeviceItem)myDriveObject.Parent;
            //_logText = test.Name.ToString();

            //MainWindow myWindow = new MainWindow(_logText);
            //myWindow.ShowDialog();
        }

        /// <summary>
        /// The method contains the Drive Parameter Interconnections.
        /// <para>DriveObject actDriveObject:
        /// the driveobject, on which a rightclick was performed in TIA
        /// </para>
        /// </summary>
        public void AdjustParameters(DriveObject actDriveObject)
        {
            #region get the Drive Objects in case of CU3x0-2 drives
            /*
             * the Drive Object of the Control Unit of 
             * the selected drive axis in TIA
             */
            DriveObject myControlUnit = null;

            /*
             * the Drive Object of any other axis in 
             * the same device
             */
            DriveObject DriveAxis1 = null;
            DriveObject DriveAxis2 = null;
            DriveObject DriveAxis3 = null;
            DriveObject DriveAxis4 = null;
            DriveObject DriveAxis5 = null;

            /*
             * the Drive Object of the Infeed of 
             * the selected drive axis in TIA
             */
            DriveObject myInfeed = null;

            //get the device unit
            DeviceItem actDeviceItem =
                   (DeviceItem)actDriveObject.Parent.Parent;

            String nameOfactDeviceItem = actDeviceItem.Name.ToString();

            //In case of S120 devices
            if (actDeviceItem.TypeIdentifier == "System:Rack")
            {
                //get Control Unit Drive Object
                myControlUnit = GetControlUnit(actDriveObject);

                //get Infeed Drive Object
                myInfeed = GetInfeedAxis(actDriveObject);

                /*
                 * to access any other DriveAxis, replace the string
                 * by the name of the other drive axis 
                 */
                DriveAxis1 = GetDriveAxisByName(actDriveObject,
                    "Other_Drive_axis_name");
                DriveAxis2 = GetDriveAxisByName(actDriveObject,
                    "Other_Drive_axis_name");
                DriveAxis3 = GetDriveAxisByName(actDriveObject,
                    "Other_Drive_axis_name");
                DriveAxis4 = GetDriveAxisByName(actDriveObject,
                    "Other_Drive_axis_name");
                DriveAxis5 = GetDriveAxisByName(actDriveObject,
                    "Other_Drive_axis_name");
            }
            #endregion

            /*
             * Explanation: Access the drive object parameters by the following
             * DriveParameterCompositions:
             * 
             * In case of the Selected Drive Axis in Startdrive
             *      access via-> selectedDrive
             * 
             * In case of CU3x0-based drives: 
             *    -  ControlUnit Parameters access via-> myControlUnit
             *    -  Infeed Parameters access via-> myInfeed
             *    -  other drive axis parameters access via-> DriveAxis1 ,2,3,4,5
             *          but replcace code lines 219, 221, 223, 225, 227
             *          by the name of the axis as a string
             */

            #region Interconnections on selected drive axis 
            #region Setting-Parameters

            //SetParameter(actDriveObject, 2000, 1234);
            _logText = _logText + nameOfactDeviceItem + ":p2000 = " +
                        ReadParameterValue(actDriveObject, 2000) + Environment.NewLine;

            #endregion

            #region BICO connections (source and sinc from same selected axis)

            //ConnectParameter(actDriveObject, "840[0]", "2090.0");
            _logText = _logText + nameOfactDeviceItem + ":p840[0] = " + nameOfactDeviceItem + ":" +
                         ReadParameterValue(actDriveObject, "840[0]") + Environment.NewLine;
            WriteLog("test2");
            #endregion
            #endregion

            #region valid for CU3x0-2 based drives (multiple drive objects)
            #region BICO connections (source and sinc from different drive objects)

            //ConnectParameter(actDriveObject, "844[0]", DriveAxis1, "899.2");

            //ConnectParameter(DriveAxis1, "840[0]", DriveAxis1, "899.2");

            //ConnectParameter(myInfeed, "840[0]", DriveAxis1, "722.0");

            #endregion
            #endregion

            #region set Telegrams 

            //Here you can create any MainTelegram on an axis
            //SetMainTelegramNumber(actDriveObject, 1);   
            //SetMainTelegramNumber(actDriveObject, 105);  
            //SetMainTelegramNumber(myInfeed, 370);       
            //SetMainTelegramNumber(myControlUnit, 390);

            //Here you can change to or set a free telegram for the axis
            //SetFreeTelegram(actDriveObject, true, 10, 10, false);

            //Here you can add a telegram extension to the axis
            //AddMainTelegramExtension(actDriveObject, 2,3, false);

            //Here you can insert an additional free telegram to the axis
            //AddAdditionalTelegram(actDriveObject,1,2);

            //Here you can add a safety telegram
            //AddSafetyTelegram(actDriveObject, 30);
            //AddSafetyTelegram(actDriveObject, 31);
            //AddSafetyTelegram(actDriveObject, 901);
            //AddSafetyTelegram(actDriveObject, 902);
            //AddSafetyTelegram(actDriveObject, 903);

            //Here you can add a safety info/control channel
            //AddSafetyInfoControlChannel(actDriveObject, 700);
            //AddSafetyInfoControlChannel(actDriveObject, 701);

            //Here you can add an addtional torque telegram 750
            //AddTorqueTelegram(actDriveObject);

            #endregion
        }

        /// <summary>
        /// Called when there is a mousover the button at a DeviceItem.
        /// It will be used to enable the button.
        /// <para>MenuSelectionProvider DeviceItem menuSelectionProvider:
        /// here, the same type as in addInRootSubmenu.Items.AddActionItem
        /// must be used -> here it is DeviceItem
        /// </para>
        /// </summary>
        private MenuStatus OnCanSomething(MenuSelectionProvider
            <DeviceItem> menuSelectionProvider)
        {
            //enable the button
            return MenuStatus.Enabled;
        }

        /// <summary>
        /// Will be called when the Add-In is started on the project level
        /// <para>MenuSelectionProvider Project menuSelectionProvider
        /// here, the same type as in addInRootSubmenu.Items.AddActionItem
        /// must be used -> here it is Project
        /// </para>
        /// </summary>
        private void OnClickProject(MenuSelectionProvider<Project>
            menuSelectionProvider)
        {
            //Do Nothing on Project level
        }

        /// <summary>
        /// Called when there is a mousover the button at the Project 
        /// Level. It will be used to disable the button because no 
        /// action should be performed on project level.
        /// <para>MenuSelectionProvider Project menuSelectionProvider
        /// here, the same type as in addInRootSubmenu.Items.AddActionItem
        /// must be used -> here it is Project
        /// </para>
        /// </summary>
        private MenuStatus OnStatusUpdateProject(MenuSelectionProvider
            <Project> menuSelectionProvider)
        {
            //disable the button
            return MenuStatus.Disabled;
        }
    }
}