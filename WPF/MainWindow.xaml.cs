using System;
using System.Windows;
using System.Collections.Generic;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(String logText)
        {
            InitializeComponent();

            //load a new page to the Mainwindow
            frame.Content = new Page1(this, logText);
        }

        /// <summary>
        /// Minimize the Window
        /// </summary>
        private void btnMinimizeScreen_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Focus();
            this.WindowState = WindowState.Minimized;

        }

        /// <summary>
        /// Maximize the Window
        /// </summary>
        private void btnMaximizeScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.MinimizeRectangle1.Visibility = Visibility.Visible;
                this.MinimizeRectangle2.Visibility = Visibility.Visible;
                this.MaximizeRectangle1.Visibility = Visibility.Hidden;
                this.MaximizeRectangle2.Visibility = Visibility.Hidden;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.MinimizeRectangle1.Visibility = Visibility.Hidden;
                this.MinimizeRectangle2.Visibility = Visibility.Hidden;
                this.MaximizeRectangle1.Visibility = Visibility.Visible;
                this.MaximizeRectangle2.Visibility = Visibility.Visible;

            }
            this.frame.Focus();

        }

        /// <summary>
        /// Reset the Scrollbar
        /// </summary>
        public void SetMyScrollViewerToTop()
        {
            myScrollviewer.ScrollToHorizontalOffset(0);
            myScrollviewer.ScrollToVerticalOffset(0);
        }

        /// <summary>
        /// Close the Window
        /// </summary>
        private void btnCloseScreen_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Focus();
            closeWindow();
        }


        public void closeWindow()
        {
            Close();
        }

        /// <summary>
        /// Set the title of the Window
        /// </summary>
        public void setTitle(String newTitle)
        {
            this.Title= newTitle;
        }
    }
}
