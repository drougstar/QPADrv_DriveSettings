using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ClosedXML.Excel;
using Siemens.Engineering.HW;
using System.Windows;

namespace WPF
{
    public partial class ExportPage : Page
    {
        private string _driveName;
        private MainWindow _mainWindow;
        private Dictionary<string, bool> selectedData;

        public ExportPage(string driveName, MainWindow mainWindow)
        {
            InitializeComponent();
            _driveName = driveName;
            _mainWindow = mainWindow;
            selectedData = new Dictionary<string, bool>
            {
                {"ParameterData", false},
                {"TelegramData", false},
                {"AxisData", false}
            };
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            selectedData["ParameterData"] = chkParameterData.IsChecked ?? false;
            selectedData["TelegramData"] = chkTelegramData.IsChecked ?? false;
            selectedData["AxisData"] = chkAxisData.IsChecked ?? false;

            if (!selectedData.ContainsValue(true))
            {
                MessageBox.Show("Please select at least one data type to export.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExportToExcel();
            MessageBox.Show($"Data exported successfully for {_driveName}.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            _mainWindow.NavigateToDriveList(_driveName, "Export completed.");
        }

        private void ExportToExcel()
        {
            string filePath = $"{_driveName}_Export_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            using (var workbook = new XLWorkbook())
            {
                if (selectedData["ParameterData"])
                {
                    var wsParam = workbook.Worksheets.Add("Parameter Data");
                    wsParam.Cell(1, 1).Value = "Parameter";
                    wsParam.Cell(1, 2).Value = "Value";
                    wsParam.Cell(2, 1).Value = "p2000";
                    wsParam.Cell(2, 2).Value = "3500";
                }

                if (selectedData["TelegramData"])
                {
                    var wsTelegram = workbook.Worksheets.Add("Telegram Data");
                    wsTelegram.Cell(1, 1).Value = "Telegram";
                    wsTelegram.Cell(1, 2).Value = "Address";
                    wsTelegram.Cell(2, 1).Value = "105";
                    wsTelegram.Cell(2, 2).Value = "0x1A";
                }

                if (selectedData["AxisData"])
                {
                    var wsAxis = workbook.Worksheets.Add("Axis Data");
                    wsAxis.Cell(1, 1).Value = "Axis";
                    wsAxis.Cell(1, 2).Value = "Status";
                    wsAxis.Cell(2, 1).Value = "Axis1";
                    wsAxis.Cell(2, 2).Value = "Active";
                }

                workbook.SaveAs(filePath);
            }
        }
    }
}
