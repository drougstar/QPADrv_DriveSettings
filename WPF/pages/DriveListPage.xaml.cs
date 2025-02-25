using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class DriveListPage : Page
    {
        private string selectedDrive;
        private MainWindow mainWindow;
        private string logText;

        public DriveListPage(string driveType, MainWindow window, string logText)
        {
            selectedDrive = driveType;
            mainWindow = window;
            this.logText = logText;
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            StackPanel mainPanel = new StackPanel { Margin = new Thickness(20) };
            TextBlock title = new TextBlock { Text = $"Drive List for {selectedDrive}:", FontSize = 18 };
            mainPanel.Children.Add(title);

            ListView driveListView = new ListView { Height = 300 };
            driveListView.Items.Add($"{selectedDrive} - Drive 1");
            driveListView.Items.Add($"{selectedDrive} - Drive 2");
            driveListView.Items.Add($"{selectedDrive} - Drive 3");
            mainPanel.Children.Add(driveListView);

            Button backButton = new Button { Content = "Back", Width = 100 };
            backButton.Click += (s, e) => mainWindow.frame.Content = new DriveSelectionPage(mainWindow, logText);
            mainPanel.Children.Add(backButton);

            this.Content = mainPanel;
        }
    }
}
