using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class DriveSelectionPage : Page
    {
        private MainWindow mainWindow;
        private string logText;

        public DriveSelectionPage(MainWindow window, string logText)
        {
            InitializeComponent();
            this.mainWindow = window;
            this.logText = logText;
            CreateUI();
        }

        private void CreateUI()
        {
            StackPanel mainPanel = new StackPanel { Margin = new Thickness(20) };
            TextBlock title = new TextBlock { Text = "Please Select Drive Type:", FontSize = 18 };
            mainPanel.Children.Add(title);

            RadioButton g120Radio = new RadioButton { Content = "G120", GroupName = "DriveGroup" };
            RadioButton s120VectorRadio = new RadioButton { Content = "S120 Vector", GroupName = "DriveGroup" };
            RadioButton s120ServoRadio = new RadioButton { Content = "S120 Servo", GroupName = "DriveGroup" };

            mainPanel.Children.Add(g120Radio);
            mainPanel.Children.Add(s120VectorRadio);
            mainPanel.Children.Add(s120ServoRadio);

            Button nextButton = new Button { Content = "Next", Width = 100 };
            nextButton.Click += (s, e) =>
            {
                string selectedDrive = g120Radio.IsChecked == true ? "G120" :
                                       s120VectorRadio.IsChecked == true ? "S120 Vector" :
                                       s120ServoRadio.IsChecked == true ? "S120 Servo" : null;

                if (selectedDrive != null)
                    mainWindow.NavigateToDriveList(selectedDrive, logText);
                else
                    MessageBox.Show("Please Select At Least One Item.");
            };
            mainPanel.Children.Add(nextButton);

            this.Content = mainPanel;
        }
    }
}
