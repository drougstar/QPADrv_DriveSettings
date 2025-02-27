// ============================
// STEP: Project Cleanup & Refactor
// ============================

using Siemens.Engineering.AddIn.Menu;
using Siemens.Engineering.HW;
using Siemens.Engineering;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF;

namespace WPF
{
    /// <summary>
    /// MainWindow structure refactored to match new UI requirements.
    /// Previous pages (DriveListPage, DriveSelectionPage) removed.
    /// All new pages are located inside the 'Pages' folder.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadMainPage();
        }

        /// <summary>
        /// Loads the main page with TreeView and buttons.
        /// </summary>
        private void LoadMainPage()
        {
            frame.Content = new Pages.MainPage(this); // MainPage now serves as the main landing page
        }

        /// <summary>
        /// Opens the Parameter Settings Page.s
        /// </summary>
        public void OpenParameterSettingsPage(string driveName, string driveType)
        {
            frame.Content = new Pages.ParameterSettingsPage(this, driveName, driveType);
        }

        /// <summary>
        /// Opens the Telegram Configuration Page (placeholder for future implementation).
        /// </summary>
        public void OpenTelegramConfigurationPage()
        {
            MessageBox.Show("Telegram Configuration Page - Coming Soon");
        }

        /// <summary>
        /// Opens the Function Modules Page (placeholder for future implementation).
        /// </summary>
        public void OpenFunctionModulesPage()
        {
            MessageBox.Show("Function Modules Page - Coming Soon");
        }

        /// <summary>
        /// Close the application.
        /// </summary>
        public void CloseApplication()
        {
            this.Close();
        }

        private void btnMinimizeScreen_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizeScreen_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void btnCloseScreen_Click(object sender, RoutedEventArgs e)
        {
            CloseApplication();
        }
    }
}

