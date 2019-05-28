using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindEnergy
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        MainWindow main;
        double totalPower;
        public List<Weather> weatherList;
        List<CheckBox> checkBoxes;
        List<DailyTotalPower> dailyTotalPowers;
        double modifiableTotalPower;
        double greenFareRate = 0;
        public ChartWindow()
        {
            InitializeComponent();
            main = (MainWindow)Application.Current.MainWindow;
            checkBoxes = new List<CheckBox>();
            weatherList = new List<Weather>();
            dailyTotalPowers = new List<DailyTotalPower>();
            modifiableTotalPower = 0;
            chart.Loaded += Chart_Loaded;
            totalPower = 0;
            GetGreenFareRate();
            if (main.startDate != main.endDate)
                Title = "Generated Power for " + main.startDate.ToShortDateString() + " - " + main.endDate.ToShortDateString();
            else
                Title = "Generated Power for " + main.startDate.ToShortDateString();

            if (main.comparisonOn)
            {
                compareBtn.Content = "Compare To";
                comparerLbl.Content = main.firstWeatherToCompare[0].WindGenerator.Name;
            }
            else
            {
                compareBtn.Content = "Compare";
                comparerLbl.Content = string.Empty;
            }
        }

        private void GetGreenFareRate()
        {
            //greenRateTB.Text = greenRateTB.Text.Replace(',', '.');
            bool success = double.TryParse(greenRateTB.Text, out greenFareRate);
            if (!success)
            {
                MessageBox.Show("Enter valid Green Fare Rate");
            }
        }

        private void Item_Checked(object sender, RoutedEventArgs e)
        {
            var item = chart.Series.Where(x => ((ColumnSeries)x).Title == ((CheckBox)sender).Content).FirstOrDefault();
            ((ColumnSeries)item).Visibility = Visibility.Visible;
            double dayPower = dailyTotalPowers.Where(x => x.Date.Date == (((CheckBox)sender).Content as DateTime?))
                                              .Select(p => p.TotalPower).FirstOrDefault();
            modifiableTotalPower += dayPower;
            totalLbl.Content = "Total: " + modifiableTotalPower.ToString("0.00") + "kW*h";
            greenFareLbl.Content = (modifiableTotalPower * greenFareRate).ToString("0.00") + " €";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var item = chart.Series.Where(x => ((ColumnSeries)x).Title == ((CheckBox)sender).Content).FirstOrDefault();
            ((ColumnSeries)item).Visibility = Visibility.Hidden;
            double dayPower = dailyTotalPowers.Where(x => x.Date.Date == (((CheckBox)sender).Content as DateTime?))
                                              .Select(p => p.TotalPower).FirstOrDefault();
            modifiableTotalPower -= dayPower;
            totalLbl.Content = "Total: " + modifiableTotalPower.ToString("0.00") + "kW*h";
            greenFareLbl.Content = (modifiableTotalPower * greenFareRate).ToString("0.00") + " €";
        }

        private void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            GetPowerValues();
        }

        public void GetPowerValues()
        {
            dailyTotalPowers.Clear();
            legend.Children.Clear();
            checkBoxes.Clear();
            if ((bool)main.dailyRB.IsChecked)
                weatherList = main.weatherListDaily;
            if ((bool)main.hourlyRB.IsChecked)
                weatherList = main.weatherListHourly;
            chart.Series.Clear();
            List<int> dates = main.weatherListHourly.Select(x => x.date.DayOfYear).Distinct().ToList();
            totalPower = 0;
            foreach (var day in dates)
            {
                List<KeyValuePair<string, double>> dataList = new List<KeyValuePair<string, double>>();
                foreach (var weather in weatherList.Where(x => x.date.DayOfYear == day))
                {
                    if (weather.date.DayOfYear == day)
                    {
                        double power = 0;
                        if ((bool)main.hourlyRB.IsChecked)
                        {
                            power = main.selectedWindGen.CalculatePowerForHour(weather.windSpeed);
                            //if(main.selectedWindGen.RatedWindSpeed == weather.windSpeed-1 || main.selectedWindGen.RatedWindSpeed == weather.windSpeed + 1)
                            //{
                            //    MessageBox.Show("Calculated Power is: " + power + "\n Rated Power is: " + main.selectedWindGen.RatedPower);
                            //}
                            weather.WindGenerator = InputGeneratorInfo(power);
                            dataList.Add(new KeyValuePair<string, double>(weather.date.ToShortTimeString(), power));
                        }
                        else if ((bool)main.dailyRB.IsChecked)
                        {
                            power = main.selectedWindGen.CalculatePowerForDay(weather.windSpeed);
                            weather.WindGenerator = InputGeneratorInfo(power);
                            dataList.Add(new KeyValuePair<string, double>(weather.date.ToShortDateString(), power));
                        }

                        totalPower += power;
                    }
                }
                ColumnSeries series = new ColumnSeries()
                {
                    Title = weatherList.Where(x => x.date.DayOfYear == day).Select(d => d.date).FirstOrDefault(),
                    IndependentValuePath = "Key",
                    DependentValuePath = "Value",
                    ItemsSource = dataList,

                };
                dailyTotalPowers.Add(new DailyTotalPower()
                {
                    Date = weatherList.Where(x => x.date.DayOfYear == day).Select(d => d.date).FirstOrDefault(),
                    TotalPower = totalPower
                });
                totalPower = 0;
                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = true;
                checkBox.Content = series.Title;
                checkBox.Checked += Item_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;
                checkBoxes.Add(checkBox);
                ColumnDefinition column = new ColumnDefinition();
                RowDefinition row = new RowDefinition();
                column.Width = new GridLength(200);
                row.Height = new GridLength(20);
                legend.ColumnDefinitions.Add(column);
                legend.RowDefinitions.Add(row);
                Grid.SetRow(checkBox, checkBoxes.Count - 1);
                Grid.SetColumn(checkBox, 0);

                legend.Children.Add(checkBox);
                chart.Series.Add(series);
            }
            legend.Height = checkBoxes.Count * 20;
            totalPower = dailyTotalPowers.Select(x => x.TotalPower).Sum();
            modifiableTotalPower = totalPower;
            totalLbl.Content = "Total: " + totalPower.ToString("0.00") + "kW*h";
            greenFareLbl.Content = (totalPower * greenFareRate).ToString("0.00") + " €";
        }

        WindGenerator InputGeneratorInfo(double power)
        {
            return new WindGenerator()
            {
                Power = power,
                Name = main.selectedWindGen.Name,
                ErrorMessage = main.selectedWindGen.ErrorMessage,
                RatedPower = main.selectedWindGen.RatedPower,
                RatedWindSpeed = main.selectedWindGen.RatedWindSpeed,
                WindSpeed = main.selectedWindGen.WindSpeed,
                Height = main.selectedWindGen.Height,
                Price = main.selectedWindGen.Price
            };
        }

        public void CheckAllDates()
        {
            foreach (var item in legend.Children)
            {
                if (item is CheckBox)
                {
                    ((CheckBox)item).IsChecked = true;
                }
            }
        }

        private void DailyRB_Checked(object sender, RoutedEventArgs e)
        {
            main.dailyRB.IsChecked = dailyRB.IsChecked;
            CheckAllDates();
            GetPowerValues();
        }

        private void HourlyRB_Checked(object sender, RoutedEventArgs e)
        {
            main.hourlyRB.IsChecked = hourlyRB.IsChecked;
            CheckAllDates();
            GetPowerValues();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            UIElement toDelete = new UIElement();
            if (printCanvas.RowDefinitions.Count > 1)
            {
                printCanvas.RowDefinitions.RemoveAt(1);

                foreach (UIElement item in printCanvas.Children)
                {
                    if (Grid.GetRow(item) == 1)
                        toDelete = item;
                }
                printCanvas.Children.Remove(toDelete);
            }
            Grid grid = new Grid();
            RowDefinition row = new RowDefinition()
            {
                Height = new GridLength(0, GridUnitType.Star)
            };
            printCanvas.RowDefinitions.Add(row);
            grid.Children.Add(GenerateGrid());
            Grid.SetRow(grid, 1);
            Grid.SetColumn(grid, 0);
            printCanvas.Children.Add(grid);
            scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            PrintDialog printDialog = new PrintDialog();
            scroller.Content = printCanvas;
            scroller.ScrollToTop();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.UserPageRangeEnabled = true;
                printDialog.PrintVisual(scroller.Content as Visual, "Printing");
            }
        }

        private Grid GenerateGrid()
        {
            DateTime? currentDate = null;
            double dailyTotal = 0;
            //Margin
            Thickness thickness = new Thickness(5, 5, 5, 5);

            Grid docGrid = new Grid();
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(40);
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column4 = new ColumnDefinition();
            column4.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column5 = new ColumnDefinition();
            column5.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column6 = new ColumnDefinition();
            column6.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column7 = new ColumnDefinition();
            column7.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column8 = new ColumnDefinition();
            column8.Width = new GridLength(350, GridUnitType.Auto);
            ColumnDefinition column9 = new ColumnDefinition();
            column9.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column10 = new ColumnDefinition();
            column10.Width = new GridLength(250, GridUnitType.Auto);
            ColumnDefinition column11 = new ColumnDefinition();
            column11.Width = new GridLength(250, GridUnitType.Auto);

            docGrid.RowDefinitions.Add(row1);
            docGrid.ColumnDefinitions.Add(column);
            docGrid.ColumnDefinitions.Add(column1);
            docGrid.ColumnDefinitions.Add(column2);
            docGrid.ColumnDefinitions.Add(column3);
            docGrid.ColumnDefinitions.Add(column4);
            docGrid.ColumnDefinitions.Add(column5);
            docGrid.ColumnDefinitions.Add(column6);
            docGrid.ColumnDefinitions.Add(column7);
            docGrid.ColumnDefinitions.Add(column8);
            docGrid.ColumnDefinitions.Add(column9);
            docGrid.ColumnDefinitions.Add(column10);
            docGrid.ColumnDefinitions.Add(column11);

            TextBlock name = new TextBlock();
            name.Text = "Name";
            TextBlock date = new TextBlock();
            date.Text = "Date";
            TextBlock power = new TextBlock();
            power.Text = "Power (kW*h)";
            TextBlock wind = new TextBlock();
            wind.Text = "Wind Speed (m/s)";
            TextBlock error = new TextBlock();
            error.Text = "Error";
            TextBlock hours = new TextBlock();
            hours.Text = "Hour";
            TextBlock rPower = new TextBlock();
            rPower.Text = "Rated Power (W)";
            TextBlock rWind = new TextBlock();
            rWind.Text = "Rated Wind Speed (m/s)";
            TextBlock location = new TextBlock();
            location.Text = "Location";
            TextBlock total = new TextBlock();
            total.Text = "Total (kW*h)";
            TextBlock rate = new TextBlock();
            rate.Text = "Green Rate (€/kW*h)";
            TextBlock totalGreen = new TextBlock();
            totalGreen.Text = "Green Fare (€)";

            List<TextBlock> headers = new List<TextBlock>();
            headers.Add(name);
            headers.Add(date);
            headers.Add(hours);
            headers.Add(wind);
            headers.Add(power);
            headers.Add(error);
            headers.Add(rPower);
            headers.Add(rWind);
            headers.Add(location);
            headers.Add(total);
            headers.Add(rate);
            headers.Add(totalGreen);

            for (int i = 0; i < headers.Count; i++)
            {
                headers[i].HorizontalAlignment = HorizontalAlignment.Center;
                headers[i].Margin = new Thickness(5, 5, 5, 5);
                Grid.SetRow(headers[i], 0);
                Grid.SetColumn(headers[i], i);
                docGrid.Children.Add(headers[i]);
            }

            for (int i = 0; i <= headers.Count; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Stroke = Brushes.Black;
                rectangle.Fill = Brushes.Transparent;
                Grid.SetRow(rectangle, 0);
                Grid.SetColumn(rectangle, i);
                docGrid.Children.Add(rectangle);
            }


            for (int i = 0; i < weatherList.Count; i++)
            {
                if (currentDate == null)
                    currentDate = weatherList[i].date;
                if (currentDate != weatherList[i].date.Date || (i + 1) == weatherList.Count)
                {
                    RowDefinition dailyTotalRow = new RowDefinition();
                    dailyTotalRow.Height = new GridLength(30, GridUnitType.Auto);
                    docGrid.RowDefinitions.Add(dailyTotalRow);
                    ColumnDefinition columnDailyTotal = new ColumnDefinition();
                    docGrid.ColumnDefinitions.Add(columnDailyTotal);
                    TextBlock totalDailyNum = new TextBlock();
                    totalDailyNum.FontWeight = FontWeights.Bold;
                    totalDailyNum.Text = dailyTotal.ToString("0.00");
                    totalDailyNum.HorizontalAlignment = HorizontalAlignment.Center;
                    totalDailyNum.Margin = thickness;
                    Grid.SetRow(totalDailyNum, i + 1);
                    Grid.SetColumn(totalDailyNum, 9);
                    docGrid.Children.Add(totalDailyNum);

                    dailyTotal = 0;
                    currentDate = weatherList[i].date;
                }

                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(30);
                docGrid.RowDefinitions.Add(row);
                ColumnDefinition columnName = new ColumnDefinition();
                columnName.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnDate = new ColumnDefinition();
                columnDate.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnHour = new ColumnDefinition();
                columnHour.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnWind = new ColumnDefinition();
                columnWind.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnGen = new ColumnDefinition();
                columnGen.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnError = new ColumnDefinition();
                columnError.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnRatedPower = new ColumnDefinition();
                columnGen.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnRatedWind = new ColumnDefinition();
                columnRatedWind.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnLocation = new ColumnDefinition();
                columnLocation.Width = new GridLength(350, GridUnitType.Auto);
                ColumnDefinition columnGreenRate = new ColumnDefinition();
                columnGreenRate.Width = new GridLength(250, GridUnitType.Auto);
                ColumnDefinition columnGreenTotal = new ColumnDefinition();
                columnGreenTotal.Width = new GridLength(250, GridUnitType.Auto);

                docGrid.ColumnDefinitions.Add(columnName);
                docGrid.ColumnDefinitions.Add(columnDate);
                docGrid.ColumnDefinitions.Add(columnHour);
                docGrid.ColumnDefinitions.Add(columnWind);
                docGrid.ColumnDefinitions.Add(columnGen);
                docGrid.ColumnDefinitions.Add(columnError);
                docGrid.ColumnDefinitions.Add(columnRatedPower);
                docGrid.ColumnDefinitions.Add(columnRatedWind);
                docGrid.ColumnDefinitions.Add(columnLocation);
                docGrid.ColumnDefinitions.Add(columnGreenRate);
                docGrid.ColumnDefinitions.Add(columnGreenTotal);

                TextBlock nameText = new TextBlock();
                nameText.Text = weatherList[i].WindGenerator.Name;
                nameText.HorizontalAlignment = HorizontalAlignment.Center;
                nameText.Margin = thickness;
                Grid.SetRow(nameText, i + 1);
                Grid.SetColumn(nameText, 0);
                TextBlock dateText = new TextBlock();
                dateText.Text = weatherList[i].date.ToLongDateString();
                dateText.HorizontalAlignment = HorizontalAlignment.Center;
                dateText.Margin = thickness;
                Grid.SetRow(dateText, i + 1);
                Grid.SetColumn(dateText, 1);
                TextBlock hourText = new TextBlock();
                hourText.Text = weatherList[i].date.ToLongTimeString();
                hourText.HorizontalAlignment = HorizontalAlignment.Center;
                hourText.Margin = thickness;
                Grid.SetRow(hourText, i + 1);
                Grid.SetColumn(hourText, 2);
                TextBlock windText = new TextBlock();
                windText.Text = weatherList[i].windSpeed.ToString("0.00");
                windText.HorizontalAlignment = HorizontalAlignment.Center;
                windText.Margin = thickness;
                Grid.SetRow(windText, i + 1);
                Grid.SetColumn(windText, 3);
                TextBlock genText = new TextBlock();
                genText.Text = weatherList[i].WindGenerator.Power.ToString("0.00");
                genText.HorizontalAlignment = HorizontalAlignment.Center;
                genText.Margin = thickness;
                dailyTotal += weatherList[i].WindGenerator.Power;
                Grid.SetRow(genText, i + 1);
                Grid.SetColumn(genText, 4);
                TextBlock errorText = new TextBlock();
                errorText.Text = weatherList[i].WindGenerator.ErrorMessage;
                errorText.HorizontalAlignment = HorizontalAlignment.Center;
                errorText.Margin = thickness;
                Grid.SetRow(errorText, i + 1);
                Grid.SetColumn(errorText, 5);
                TextBlock ratedPowerText = new TextBlock();
                ratedPowerText.Text = weatherList[i].WindGenerator.RatedPower.ToString();
                ratedPowerText.HorizontalAlignment = HorizontalAlignment.Center;
                ratedPowerText.Margin = thickness;
                Grid.SetRow(ratedPowerText, i + 1);
                Grid.SetColumn(ratedPowerText, 6);
                TextBlock ratedWindText = new TextBlock();
                ratedWindText.Text = weatherList[i].WindGenerator.RatedWindSpeed.ToString();
                ratedWindText.HorizontalAlignment = HorizontalAlignment.Center;
                ratedWindText.Margin = thickness;
                Grid.SetRow(ratedWindText, i + 1);
                Grid.SetColumn(ratedWindText, 7);
                TextBlock locationText = new TextBlock();
                locationText.Text = weatherList[i].Area.Name == null ? "N/A" : weatherList[i].Area.Name;
                locationText.HorizontalAlignment = HorizontalAlignment.Center;
                locationText.Margin = thickness;
                Grid.SetRow(locationText, i + 1);
                Grid.SetColumn(locationText, 8);
                TextBlock greenRate = new TextBlock();
                greenRate.Text = greenRateTB.Text;
                greenRate.HorizontalAlignment = HorizontalAlignment.Center;
                greenRate.Margin = thickness;
                Grid.SetRow(greenRate, i + 1);
                Grid.SetColumn(greenRate, 10);
                TextBlock greenTotal = new TextBlock();
                greenTotal.Text = (weatherList[i].WindGenerator.Power * greenFareRate).ToString("0.00");
                greenTotal.HorizontalAlignment = HorizontalAlignment.Center;
                greenTotal.Margin = thickness;
                Grid.SetRow(greenTotal, i + 1);
                Grid.SetColumn(greenTotal, 11);

                docGrid.Children.Add(nameText);
                docGrid.Children.Add(dateText);
                docGrid.Children.Add(hourText);
                docGrid.Children.Add(windText);
                docGrid.Children.Add(genText);
                docGrid.Children.Add(errorText);
                docGrid.Children.Add(ratedPowerText);
                docGrid.Children.Add(ratedWindText);
                docGrid.Children.Add(locationText);
                docGrid.Children.Add(greenRate);
                docGrid.Children.Add(greenTotal);

                SolidColorBrush errorBrush = new SolidColorBrush(Colors.DarkRed);
                errorBrush.Opacity = 0.5;
                SolidColorBrush ratedWindBrush = new SolidColorBrush(Colors.DarkGreen);
                ratedWindBrush.Opacity = 0.5;

                for (int j = 0; j <= 11; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Stroke = Brushes.Black;
                    if (errorText.Text.Length > 0)
                        rectangle.Fill = errorBrush;
                    else if (double.Parse(windText.Text) >= double.Parse(ratedWindText.Text))
                        rectangle.Fill = ratedWindBrush;
                    else
                        rectangle.Fill = Brushes.Transparent;
                    Grid.SetRow(rectangle, i + 1);
                    Grid.SetColumn(rectangle, j);
                    docGrid.Children.Add(rectangle);
                }
            }
            RowDefinition totalRow = new RowDefinition();
            ColumnDefinition columnTotal = new ColumnDefinition();
            ColumnDefinition columnTotalData = new ColumnDefinition();
            docGrid.RowDefinitions.Add(totalRow);
            docGrid.ColumnDefinitions.Add(columnTotal);
            docGrid.ColumnDefinitions.Add(columnTotalData);
            TextBlock totalData = new TextBlock();
            totalData.FontWeight = FontWeights.Bold;
            totalData.Text = totalPower.ToString("0.00");
            totalData.HorizontalAlignment = HorizontalAlignment.Center;
            totalData.Margin = thickness;
            Grid.SetRow(totalData, weatherList.Count + 1);
            Grid.SetColumn(totalData, 9);
            docGrid.Children.Add(totalData);

            RowDefinition totalGreenRow = new RowDefinition();
            ColumnDefinition columnTotalGreenData = new ColumnDefinition();
            docGrid.RowDefinitions.Add(totalGreenRow);
            docGrid.ColumnDefinitions.Add(columnTotalGreenData);
            TextBlock totalGreenData = new TextBlock();
            totalGreenData.FontWeight = FontWeights.Bold;
            totalGreenData.Text = greenFareLbl.Content.ToString();
            totalGreenData.HorizontalAlignment = HorizontalAlignment.Center;
            totalGreenData.Margin = thickness;
            Grid.SetRow(totalGreenData, weatherList.Count + 1);
            Grid.SetColumn(totalGreenData, 11);
            docGrid.Children.Add(totalGreenData);

            return docGrid;
        }

        private void CompareBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!main.comparisonOn)
            {
                main.firstWeatherToCompare.Clear();
                main.comparisonOn = true;
                main.firstWeatherToCompare = main.weatherListHourly.Select(x => x.Copy()).ToList();
                MessageBox.Show("Choose another Wind Generator to compare and click Calculate Power Button\n" +
                    " You can choose another location and Date Range, but it is preferably not to change this data");
                this.Close();
            }
            else //if (main.comparisonOn)
            {
                main.secondWeatherToCompare.Clear();
                main.secondWeatherToCompare = main.weatherListHourly.Select(x => x.Copy()).ToList();
                ComparisonWindow comparisonWindow = new ComparisonWindow();
                comparisonWindow.Show();
            }
        }

        private void GreenRateTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetGreenFareRate();
            greenFareLbl.Content = (modifiableTotalPower * greenFareRate).ToString("0.00") + " €";
        }
    }
}
