using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindEnergy
{
    /// <summary>
    /// Interaction logic for ComparisonWindow.xaml
    /// </summary>
    public partial class ComparisonWindow : Window
    {
        MainWindow main;
        public ComparisonWindow()
        {
            InitializeComponent();
            main = (MainWindow)Application.Current.MainWindow;
            DoComparison();
            this.Closed += WindowCLose;
        }

        private void WindowCLose(object sender, EventArgs e)
        {
            main.firstWeatherToCompare.Clear();
            main.secondWeatherToCompare.Clear();
            main.comparisonOn = false;
        }

        private void DoComparison()
        {
            if (main.firstWeatherToCompare.Count > 0 && main.secondWeatherToCompare.Count > 0)
            {
                name1.Text = main.firstWeatherToCompare[0].WindGenerator.Name;
                name2.Text = main.secondWeatherToCompare[0].WindGenerator.Name;

                if (main.firstWeatherToCompare[0].Area.Name == null)
                    location1.Text = main.firstWeatherToCompare[0].Area.Latitude + ", " + main.firstWeatherToCompare[0].Area.Longitude;

                if(main.secondWeatherToCompare[0].Area.Name == null)
                    location2.Text = main.secondWeatherToCompare[0].Area.Latitude + ", " + main.secondWeatherToCompare[0].Area.Longitude;

                if (main.firstWeatherToCompare[0].Area.Name == main.secondWeatherToCompare[0].Area.Name)
                {
                    location1.Text = "";
                    location2.Text = "";
                    similarLocation.Text = main.firstWeatherToCompare[0].Area.Name == null ? "N/A" : main.firstWeatherToCompare[0].Area.Name;
                }
                else
                {
                    location1.Text = main.firstWeatherToCompare[0].Area.Name;
                    location2.Text = main.secondWeatherToCompare[0].Area.Name;
                    similarLocation.Text = "";
                }


                if (main.firstWeatherToCompare[0].date == main.secondWeatherToCompare[0].date &&
                    main.secondWeatherToCompare.Last().date == main.firstWeatherToCompare.Last().date)
                {
                    date1.Text = "";
                    date2.Text = "";
                    similarDate.Text = main.firstWeatherToCompare[0].date.ToShortDateString() + " - " + main.firstWeatherToCompare.Last().date.ToShortDateString();
                }
                else
                {
                    date1.Text = main.firstWeatherToCompare[0].date.ToShortDateString() + " - " + main.firstWeatherToCompare.Last().date.ToShortDateString();
                    date2.Text = main.secondWeatherToCompare[0].date.ToShortDateString() + " - " + main.secondWeatherToCompare.Last().date.ToShortDateString();
                    similarLocation.Text = "";
                }


                ratedPower1.Text = main.firstWeatherToCompare[0].WindGenerator.RatedPower.ToString();
                ratedPower2.Text = main.secondWeatherToCompare[0].WindGenerator.RatedPower.ToString();
                double compareRatedPowerNum = main.firstWeatherToCompare[0].WindGenerator.RatedPower - main.secondWeatherToCompare[0].WindGenerator.RatedPower;
                if (compareRatedPowerNum > 0)
                {
                    compareRatedPower.Foreground = new SolidColorBrush(Colors.Green);
                    compareRatedPower.Text = "+" + compareRatedPowerNum;
                }
                else
                {
                    compareRatedPower.Foreground = new SolidColorBrush(Colors.Red);
                    compareRatedPower.Text = compareRatedPowerNum.ToString();
                }

                ratedWind1.Text = main.firstWeatherToCompare[0].WindGenerator.RatedWindSpeed.ToString();
                ratedWind2.Text = main.secondWeatherToCompare[0].WindGenerator.RatedWindSpeed.ToString();
                double ratedWSNum = main.firstWeatherToCompare[0].WindGenerator.RatedWindSpeed - main.secondWeatherToCompare[0].WindGenerator.RatedWindSpeed;
                if (ratedWSNum > 0)
                {
                    compareRatedWind.Foreground = new SolidColorBrush(Colors.Red);
                    compareRatedWind.Text = "+" + ratedWSNum;
                }
                else
                {
                    compareRatedWind.Foreground = new SolidColorBrush(Colors.Green);
                    compareRatedWind.Text = ratedWSNum.ToString();
                }

                double total1Num = main.firstWeatherToCompare.Select(x => x.WindGenerator.Power).Sum();
                double total2Num = main.secondWeatherToCompare.Select(x => x.WindGenerator.Power).Sum();
                total1.Text = total1Num.ToString("0.00");
                total2.Text = total2Num.ToString("0.00");
                double compareTotalPower = total1Num - total2Num;
                if (compareTotalPower > 0)
                {
                    comparePower.Foreground = new SolidColorBrush(Colors.Green);
                    comparePower.Text = "+" + compareTotalPower.ToString("0.00");
                }
                else
                {
                    comparePower.Foreground = new SolidColorBrush(Colors.Red);
                    comparePower.Text = compareTotalPower.ToString("0.00");
                }

                double averagePower1Num = main.firstWeatherToCompare.Select(x => x.WindGenerator.Power).Average();
                double averagePower2Num = main.secondWeatherToCompare.Select(x => x.WindGenerator.Power).Average();
                averagePower1.Text = averagePower1Num.ToString("0.00");
                averagePower2.Text = averagePower2Num.ToString("0.00");
                double compareAveragePowerNum = averagePower1Num - averagePower2Num;
                if (compareAveragePowerNum > 0)
                {
                    compareAveragePower.Foreground = new SolidColorBrush(Colors.Green);
                    compareAveragePower.Text = "+" + compareAveragePowerNum.ToString("0.00");
                }
                else
                {
                    compareAveragePower.Foreground = new SolidColorBrush(Colors.Red);
                    compareAveragePower.Text = compareAveragePowerNum.ToString("0.00");
                }

                double averageWS1Num = main.firstWeatherToCompare.Select(x => x.WindGenerator.WindSpeed).Average();
                double averageWS2Num = main.secondWeatherToCompare.Select(x => x.WindGenerator.WindSpeed).Average();
                averageWS1.Text = averageWS1Num.ToString("0.00");
                averageWS2.Text = averageWS2Num.ToString("0.00");
                double compareWinds = averageWS1Num - averageWS2Num;
                if (compareWinds > 0)
                {
                    compareAverageWS.Foreground = new SolidColorBrush(Colors.Green);
                    compareAverageWS.Text = "+" + compareWinds.ToString("0.00");
                }
                else
                {
                    compareAverageWS.Foreground = new SolidColorBrush(Colors.Red);
                    compareAverageWS.Text = compareWinds.ToString("0.00");
                }

                int? height1Num = main.firstWeatherToCompare[0].WindGenerator.Height;
                int? height2Num = main.secondWeatherToCompare[0].WindGenerator.Height;
                height1.Text = height1Num == null ? "N/A" : height1Num.ToString();
                height2.Text = height2Num == null ? "N/A" : height2Num.ToString();
                int compareHeightsNum = 0;
                if (height1Num != null && height2Num != null)
                {
                    compareHeightsNum = (int)height1Num - (int)height2Num;
                }
                if (compareHeightsNum > 0)
                {
                    compareHeights.Foreground = new SolidColorBrush(Colors.Green);
                    compareHeights.Text = "+" + compareHeightsNum.ToString();
                }
                else
                {
                    compareHeights.Foreground = new SolidColorBrush(Colors.Red);
                    compareHeights.Text = compareHeightsNum.ToString();
                }

                decimal? priceNum1 = main.firstWeatherToCompare[0].WindGenerator.Price;
                decimal? priceNum2 = main.secondWeatherToCompare[0].WindGenerator.Price;
                price1.Text = priceNum1 == null ? "N/A" : priceNum1.ToString();
                price2.Text = priceNum2 == null ? "N/A" : priceNum2.ToString();
                decimal comparePricesNum = 0;
                if (priceNum1 != null && priceNum1 > 0 && priceNum2 != null && priceNum2 > 0)
                {
                    comparePricesNum = (decimal)priceNum1 - (decimal)priceNum2;
                    if (comparePricesNum > 0)
                    {
                        comparePrices.Text = "+" + comparePricesNum.ToString();
                        comparePrices.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        comparePrices.Foreground = new SolidColorBrush(Colors.Green);
                        comparePrices.Text = comparePricesNum.ToString();
                    }
                }
            }
        }
    }
}
