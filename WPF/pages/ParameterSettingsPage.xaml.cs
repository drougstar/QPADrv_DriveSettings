using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF;

namespace WPF.Pages
{
    public partial class ParameterSettingsPage : Page
    {
        private MainWindow _mainWindow;
        private string _driveName;
        private StackPanel _parameterPanel;
        private Button _openParametersButton;

        public ParameterSettingsPage(MainWindow mainWindow, string driveName, string driveType)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _driveName = driveName;

            _parameterPanel = new StackPanel { Margin = new Thickness(10) };
            _openParametersButton = new Button
            {
                Content = "Open Parameters",
                Height = 30,
                Margin = new Thickness(10)
            };
            _openParametersButton.Click += OpenParametersButton_Click;

            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto }
                },
                Children =
                {
                    _parameterPanel,
                    _openParametersButton
                }
            };
            Grid.SetRow(_parameterPanel, 0);
            Grid.SetRow(_openParametersButton, 1);
        }

        private void OpenParametersButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDummyParameters(_driveName);
        }

        private void LoadDummyParameters(string driveType)
        {
            _parameterPanel.Children.Clear();

            var dummyParameters = new Dictionary<string, string>();
            if (driveType.StartsWith("G120"))
            {
                dummyParameters = new Dictionary<string, string>
                {
                    { "p2000", "3500" },
                    { "p840[0]", "Enabled" }
                };
            }
            else if (driveType.StartsWith("S120"))
            {
                dummyParameters = new Dictionary<string, string>
                {
                    { "p3000", "4200" },
                    { "p860[1]", "Disabled" }
                };
            }

            foreach (var param in dummyParameters)
            {
                _parameterPanel.Children.Add(new TextBlock
                {
                    Text = $"{param.Key}: {param.Value}",
                    FontSize = 14,
                    Margin = new Thickness(5)
                });
            }
        }
    }
}


// ============================
// 🎯 Updates Implemented:
// - Parameter Settings Page now includes 'Open Parameters' button.
// - Displays dummy parameters based on selected drive type.
// - Back button functionality will be implemented next.
// ============================

// ✅ Next Step:
// - Test Open Parameters button functionality.
// - Confirm parameters display correctly per drive type.
// - Provide feedback for final adjustments before adding Back button and future Compare/Export features.