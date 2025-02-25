using System;
using System.Windows;
using System.Windows.Navigation;
using System.Collections.Generic;
using Siemens.Engineering.HW; // Needed namespace for DeviceItem

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Checks Export and Start Add-In operations
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _logText;

        public MainWindow(string logText)
        {
            InitializeComponent();
            _logText = logText;

            if (frame != null)
            {
                frame.Content = new DriveSelectionPage(this, _logText);
            }
            else
            {
                throw new Exception("'frame' item should be match with x:Name for MainWindow.xaml.");
            }
        }

        /// <summary>
        /// Used to open ExportPage.
        /// </summary>
        public void OpenExportPage(string deviceName)
        {
            ExportPage exportPage = new ExportPage(deviceName, this); // Main Window Referance added.
            frame.Content = exportPage;
        }


        /// <summary>
        /// Used to open DriveListPage
        /// </summary>
        public void NavigateToDriveList(string deviceName, string logText)
        {
            if (frame != null)
            {
                frame.Content = new DriveListPage(deviceName, this, logText); // Corrected as String for parameter type.
            }
        }

        /// <summary>
        /// When started from TIA Portal, triggers Export or Drive List operation.
        /// Corrected as String for parameter type.
        /// </summary>
        public void HandleAddInAction(string actionType, string deviceName)
        {
            if (string.Equals(actionType, "Export", StringComparison.OrdinalIgnoreCase))
            {
                OpenExportPage(deviceName);
            }
            else if (string.Equals(actionType, "Start Add-In", StringComparison.OrdinalIgnoreCase))
            {
                NavigateToDriveList(deviceName, _logText);
            }
            else
            {
                MessageBox.Show($"Unknown action: {actionType}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void closeWindow()
        {
            this.Close();
        }

        private void btnMinimizeScreen_Click(object sender, RoutedEventArgs e)
        {
            this.frame?.Focus();
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
            this.frame?.Focus();
        }

        private void btnCloseScreen_Click(object sender, RoutedEventArgs e)
        {
            this.frame?.Focus();
            closeWindow();
        }

        public void SetMyScrollViewerToTop()
        {
            myScrollviewer?.ScrollToHorizontalOffset(0);
            myScrollviewer?.ScrollToVerticalOffset(0);
        }

        public void setTitle(string newTitle)
        {
            this.Title = newTitle;
        }
    }
}
