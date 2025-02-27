// ============================
// STEP: Restored Buttons with LoadInitialRightPanel Method
// ============================

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.Pages
{
    /// <summary>
    /// MainPage displaying TreeView with drive types, checkboxes for multi-selection,
    /// and three main buttons.
    /// - Restored LoadInitialRightPanel() method to display main action buttons.
    /// - Fixed CS0165 error by initializing 'parametersDisplay' before use.
    /// - CheckBox controls added for multi-selection functionality.
    /// - Handles Checked/Unchecked events to enforce same-type drive selection.
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow _mainWindow;
        private Dictionary<string, List<string>> _dummyDrives;
        private List<string> _selectedDrives;
        private string _selectedDriveType;
        //private Frame rightPanelFrame;
        private TextBlock parametersDisplay; // ✅ Initialized at class-level to prevent CS0165 error

        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _selectedDrives = new List<string>();
            parametersDisplay = new TextBlock { Margin = new Thickness(0, 10, 0, 0), FontSize = 14, TextWrapping = TextWrapping.Wrap };

            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            treeViewDrives = new TreeView { Margin = new Thickness(10) };
            mainGrid.Children.Add(treeViewDrives);
            Grid.SetColumn(treeViewDrives, 0);

            rightPanelFrame = new Frame { NavigationUIVisibility = NavigationUIVisibility.Hidden };
            mainGrid.Children.Add(rightPanelFrame);
            Grid.SetColumn(rightPanelFrame, 1);

            Content = mainGrid;

            LoadDummyDrives();
            LoadTreeView();
            LoadInitialRightPanel();
        }

        private void LoadInitialRightPanel()
        {
            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            TextBlock header = new TextBlock
            {
                Text = "Main Action Panel",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Button btnTelegramConfig = new Button { Content = "Telegram Configuration", Height = 30, Margin = new Thickness(0, 5, 0, 5) };
            btnTelegramConfig.Click += (s, e) => MessageBox.Show("Telegram Configuration Page - Coming Soon");

            Button btnParameterSettings = new Button { Content = "Parameter Settings", Height = 30, Margin = new Thickness(0, 5, 0, 5) };
            btnParameterSettings.Click += (s, e) => LoadParameterSettingsPanel();

            Button btnFunctionModules = new Button { Content = "Function Modules", Height = 30, Margin = new Thickness(0, 5, 0, 5) };
            btnFunctionModules.Click += (s, e) => MessageBox.Show("Function Modules Page - Coming Soon");

            panel.Children.Add(header);
            panel.Children.Add(btnTelegramConfig);
            panel.Children.Add(btnParameterSettings);
            panel.Children.Add(btnFunctionModules);

            rightPanelFrame.Content = panel;
        }

        private void LoadParameterSettingsPanel()
        {
            if (_selectedDrives.Count > 0)
            {
                StackPanel panel = new StackPanel { Margin = new Thickness(20) };

                TextBlock title = new TextBlock
                {
                    Text = $"Parameter Settings for: {string.Join(", ", _selectedDrives)} ({_selectedDriveType})",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 15)
                };

                Button btnBack = new Button { Content = "Back", Height = 30, Margin = new Thickness(0, 5, 0, 5) };
                btnBack.Click += (s, e) => LoadInitialRightPanel();

                Button btnOpenParameters = new Button { Content = "Open Parameters", Height = 30, Margin = new Thickness(0, 5, 0, 5) };
                btnOpenParameters.Click += (s, e) => LoadDummyParameters(_selectedDriveType);

                panel.Children.Add(title);
                panel.Children.Add(btnBack);
                panel.Children.Add(btnOpenParameters);
                panel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });
                panel.Children.Add(parametersDisplay);

                rightPanelFrame.Content = panel;
            }
            else
            {
                MessageBox.Show("Please select at least one drive before opening parameter settings.");
            }
        }

        private void LoadDummyDrives()
        {
            _dummyDrives = new Dictionary<string, List<string>>
            {
                { "G120", new List<string> { "G120_1", "G120_2" } },
                { "S120", new List<string> { "S120_1", "S120_2" } }
            };
        }

        private void LoadTreeView()
        {
            treeViewDrives.Items.Clear();
            foreach (var group in _dummyDrives)
            {
                TreeViewItem groupItem = new TreeViewItem { Header = group.Key, IsExpanded = true };
                foreach (var drive in group.Value)
                {
                    StackPanel drivePanel = new StackPanel { Orientation = Orientation.Horizontal };
                    CheckBox driveCheckBox = new CheckBox { Content = drive, Margin = new Thickness(5, 2, 0, 2) };
                    driveCheckBox.Checked += DriveCheckBox_Checked;
                    driveCheckBox.Unchecked += DriveCheckBox_Unchecked;

                    drivePanel.Children.Add(driveCheckBox);
                    groupItem.Items.Add(drivePanel);
                }
                treeViewDrives.Items.Add(groupItem);
            }
        }

        private void DriveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox selectedCheckBox)
            {
                string selectedDrive = selectedCheckBox.Content.ToString();
                string selectedGroup = ((TreeViewItem)((StackPanel)selectedCheckBox.Parent).Parent).Header.ToString();

                if (_selectedDrives.Count == 0)
                {
                    _selectedDriveType = selectedGroup;
                    EnforceDriveSelectionRules(_selectedDriveType);
                }

                if (selectedGroup == _selectedDriveType)
                {
                    _selectedDrives.Add(selectedDrive);
                }
                else
                {
                    selectedCheckBox.IsChecked = false;
                    MessageBox.Show("Only drives of the same type can be selected.");
                }
            }
        }

        private void DriveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox deselectedCheckBox)
            {
                string deselectedDrive = deselectedCheckBox.Content.ToString();
                _selectedDrives.Remove(deselectedDrive);

                if (_selectedDrives.Count == 0)
                {
                    _selectedDriveType = null;
                    ResetDriveSelection();
                }
            }
        }

        private void EnforceDriveSelectionRules(string selectedType)
        {
            foreach (TreeViewItem groupItem in treeViewDrives.Items)
            {
                bool isSameType = groupItem.Header.ToString() == selectedType;
                foreach (StackPanel panel in groupItem.Items)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is CheckBox checkBox)
                        {
                            checkBox.IsEnabled = isSameType || checkBox.IsChecked == true;
                        }
                    }
                }
            }
        }

        private void ResetDriveSelection()
        {
            foreach (TreeViewItem groupItem in treeViewDrives.Items)
            {
                foreach (StackPanel panel in groupItem.Items)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is CheckBox checkBox)
                        {
                            checkBox.IsEnabled = true;
                        }
                    }
                }
            }
        }

        private void LoadDummyParameters(string driveType)
        {
            parametersDisplay.Text = string.Empty;
            foreach (var drive in _selectedDrives)
            {
                parametersDisplay.Text += $"Drive: {drive}\n";
                if (driveType.StartsWith("G120"))
                {
                    parametersDisplay.Text += "✔ p2000: 3500\n✔ p840[0]: Enabled\n\n";
                }
                else if (driveType.StartsWith("S120"))
                {
                    parametersDisplay.Text += "✔ p3000: 4200\n✔ p860[1]: Disabled\n\n";
                }
            }
        }
    }
}

// ============================
// 🎯 Updates:
// - Restored LoadInitialRightPanel() method with action buttons.
// - Added dynamic panel updates when buttons are clicked.
// - Parameter Settings dynamically shows selected drives and parameters.
// - TreeView multi-selection with same-type restriction preserved.
// ============================

// ✅ Next Step:
// - Rebuild and test for button visibility and functionality.
// - Verify dynamic loading of Parameter Settings panel and Back navigation.
// - Confirm TreeView selection consistency across panel switches.