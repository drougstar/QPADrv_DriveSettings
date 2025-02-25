using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        MainWindow _window;
        string _text;

        public Page1(MainWindow window, String logText)
        {
            InitializeComponent();
            button_createLog.Background = new SolidColorBrush(Color.FromRgb(221,221,221));
            _window = window;
            _window.setTitle("MessageBox");
            textBlockInterconnections.Text = logText;
            _text = logText;
        }
        /// <summary>
        /// Close the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ok_Click(object sender, RoutedEventArgs e)
        {
            _window.closeWindow();
        }

        /// <summary>
        /// Create a new log file on the Desktop named LogAddIn.txt 
        /// </summary>
        private void Button_createLog_Click(object sender, RoutedEventArgs e)
        {
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the log text to a new file named "LogAddIn.txt".
            File.WriteAllText(System.IO.Path.Combine(docPath, "LogAddIn.txt"), _text);

            //Change the buttons background color to green
            button_createLog.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }
    }
}
