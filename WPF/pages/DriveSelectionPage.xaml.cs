using System;
using System.Windows;
using System.Windows.Controls;

namespace TIA_Add_In_Project
{
    public partial class DriveSelectionPage : Page
    {
        public DriveSelectionPage()
        {
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            StackPanel mainPanel = new StackPanel { Margin = new Thickness(20) };

            TextBlock title = new TextBlock
            {
                Text = "Lütfen bir sürücü tipi seçin:",
                FontSize = 18,
                Margin = new Thickness(0, 0, 0, 10)
            };
            mainPanel.Children.Add(title);

            RadioButton g120Radio = new RadioButton { Content = "G120", GroupName = "DriveGroup", Margin = new Thickness(0, 5, 0, 5) };
            RadioButton s120VectorRadio = new RadioButton { Content = "S120 Vector", GroupName = "DriveGroup", Margin = new Thickness(0, 5, 0, 5) };
            RadioButton s120ServoRadio = new RadioButton { Content = "S120 Servo", GroupName = "DriveGroup", Margin = new Thickness(0, 5, 0, 15) };

            mainPanel.Children.Add(g120Radio);
            mainPanel.Children.Add(s120VectorRadio);
            mainPanel.Children.Add(s120ServoRadio);

            Button nextButton = new Button
            {
                Content = "Next",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            nextButton.Click += (s, e) =>
            {
                string selectedDrive = g120Radio.IsChecked == true ? "G120" :
                                       s120VectorRadio.IsChecked == true ? "S120 Vector" :
                                       s120ServoRadio.IsChecked == true ? "S120 Servo" : null;

                if (selectedDrive != null)
                {
                    NavigationService.Navigate(new DriveListPage(selectedDrive));
                }
                else
                {
                    MessageBox.Show("Lütfen bir seçenek belirleyin.");
                }
            };

            mainPanel.Children.Add(nextButton);
            this.Content = mainPanel;
        }
    }

    public partial class DriveListPage : Page
    {
        private string selectedDrive;

        public DriveListPage(string driveType)
        {
            selectedDrive = driveType;
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            StackPanel mainPanel = new StackPanel { Margin = new Thickness(20) };

            TextBlock title = new TextBlock
            {
                Text = $"{selectedDrive} için sürücü listesi:",
                FontSize = 18,
                Margin = new Thickness(0, 0, 0, 10)
            };
            mainPanel.Children.Add(title);

            ListView driveListView = new ListView { Height = 300 };
            driveListView.Items.Add($"{selectedDrive} - Sürücü 1");
            driveListView.Items.Add($"{selectedDrive} - Sürücü 2");
            driveListView.Items.Add($"{selectedDrive} - Sürücü 3");

            mainPanel.Children.Add(driveListView);

            Button backButton = new Button
            {
                Content = "Back",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            backButton.Click += (s, e) => NavigationService.GoBack();

            mainPanel.Children.Add(backButton);
            this.Content = mainPanel;
        }
    }
}
