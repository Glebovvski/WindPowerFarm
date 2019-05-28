using DarkSky.Models;
using DarkSky.Services;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WindEnergy.Windows;
using static DarkSky.Services.DarkSkyService;

namespace WindEnergy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Windows.Shapes.Path selectedRegion;
        public WindGenerator selectedWindGen;
        public ViewModel vm;
        public WeatherAPI weather;
        Location selectedLocation;
        Location previousSelectedLocation;
        public DateTime startDate = DateTime.Now;
        public DateTime endDate;
        public DateTime? prevStartDate;
        public DateTime? prevEndDate;
        public List<Weather> weatherListDaily;
        ChartWindow chartWindow;
        public List<Weather> weatherListHourly;
        public Forecast forecast;

        public bool comparisonOn = false;
        public List<Weather> firstWeatherToCompare;
        public List<Weather> secondWeatherToCompare;
        public MainWindow()
        {
            InitializeComponent();
            startDateDP.SelectedDate = startDate;
            this.DataContext = new ViewModel();
            vm = this.DataContext as ViewModel;
            weather = new WeatherAPI();
            weatherListDaily = new List<Weather>();
            weatherListHourly = new List<Weather>();
            firstWeatherToCompare = new List<Weather>();
            secondWeatherToCompare = new List<Weather>();
            WindGenList.SelectedIndex = 0;
            RefreshWindList();
            selectedWindGen = vm.WindGenerators.Where(x => x.Id == vm.WindGenerators.ToList()[WindGenList.SelectedIndex].Id).FirstOrDefault();
        }

        public void RefreshWindList()
        {
            WindGenList.ItemsSource = vm.WindGenerators.Select(x => x.Name + " (" + x.RatedPower + "W)");
            WindGenList.Items.Refresh();
        }

        private void ukraine_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selectedRegion != (System.Windows.Shapes.Path)sender)
                ((System.Windows.Shapes.Path)sender).Fill = Brushes.Blue;
        }

        private void AddWindGen_Click(object sender, RoutedEventArgs e)
        {
            AddGenWindow window = new AddGenWindow();
            window.Show();
        }

        private void WindGenList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedWindGen = vm.WindGenerators.Where(x => x.Id == vm.WindGenerators.ToList()[WindGenList.SelectedIndex].Id).FirstOrDefault();
            if (selectedWindGen != null)
            {
                nameLbl.Content = selectedWindGen.Name;
                ratedPowerLbl.Content = selectedWindGen.RatedPower;
                ratedWindSpeedLbl.Content = selectedWindGen.RatedWindSpeed;
                minWindSpeedLbl.Content = selectedWindGen.MinWindSpeed;
                maxWindSpeedLbl.Content = selectedWindGen.MaxWindSpeed;
                if (selectedWindGen.Radius == 0)
                {
                    selectedWindGen.Radius = Math.Sqrt((double)selectedWindGen.SweptArea / Math.PI);
                    vm.Modify(selectedWindGen);

                }
                rotorLbl.Content = ((double)selectedWindGen.Radius).ToString("0.00");
                if (selectedWindGen.SweptArea == 0)
                {
                    selectedWindGen.SweptArea = Math.PI * selectedWindGen.Radius * selectedWindGen.Radius;
                    vm.Modify(selectedWindGen);
                }
                sweptAreaLbl.Content = ((double)selectedWindGen.SweptArea).ToString("0.0");
                heightLbl.Content = selectedWindGen.Height == null ? "N/A" : selectedWindGen.Height.ToString();
                if (chartWindow != null)
                {
                    chartWindow.GetPowerValues();
                }
                priceLabel.Content = selectedWindGen.Price.ToString();
            }
        }


        private async void PowerRangeBtn_Click(object sender, RoutedEventArgs e)
        {
            DarkSkyResponse res;
            CheckDates();
            if (previousSelectedLocation == null || (previousSelectedLocation.Latitude != selectedLocation.Latitude ||
                previousSelectedLocation.Longitude != selectedLocation.Longitude) ||
                (prevEndDate != endDate || prevStartDate != startDate))
            {
                if (CheckSelectedRegionForBingMap() && CheckDates())
                {
                    string responce = new AsynchronousClient().SendMessage(11000, "get_location:" + selectedLocation.Latitude.ToString().Replace(",",".")+","+selectedLocation.Longitude.ToString().Replace(",", "."));
                    string[] city = responce.Split(';');
                    City bingArea = new City()
                    {
                        Name=city[0],
                        Latitude = double.Parse(city[1]),
                        Longitude = double.Parse(city[2])
                    };

                    OptionalParameters optionalParameters = new OptionalParameters()
                    {
                        MeasurementUnits = "si",
                        DataBlocksToExclude = new List<ExclusionBlock>() { ExclusionBlock.Minutely }
                    };
                    weatherListDaily.Clear();
                    weatherListHourly.Clear();
                    ProgressBarWindow progressBar = new ProgressBarWindow();
                    progressBar.Show();
                    progressBar.progress.Minimum = 0;
                    progressBar.progress.Maximum = (endDate - startDate).Days;
                    for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
                    {
                        progressBar.progress.Value++;
                        optionalParameters.ForecastDateTime = i;
                        res = await weather.darkSky.GetForecast(selectedLocation.Latitude, selectedLocation.Longitude, optionalParameters);
                        forecast = res.Response;
                        if (forecast.Daily != null || forecast.Hourly != null)
                        {
                            if (forecast.Daily.Data != null)
                                weatherListDaily.AddRange(weather.GetWindSpeed(forecast.Daily.Data, bingArea));
                            if (forecast.Hourly.Data != null)
                                weatherListHourly.AddRange(weather.GetWindSpeed(forecast.Hourly.Data, bingArea));
                        }
                    }
                    if (weatherListHourly.Count > 0)
                        ExtrapolateWeatherListForMissingDates();
                    progressBar.Close();
                    GetChartWindow();
                }
            }
            else
            {
                GetChartWindow();
            }
        }

        private void ExtrapolateWeatherListForMissingDates()
        {
            List<Weather> listOfMissingDatesHourly = new List<Weather>();
            double averageWindSpeed = weatherListHourly.Select(x => x.windSpeed).Average();
            for (DateTime start = startDate; start < weatherListHourly.First().date; start = start.AddHours(1))
            {
                listOfMissingDatesHourly.Add(new Weather()
                {
                    windSpeed = averageWindSpeed,
                    date = start,
                    Area = weatherListHourly.First().Area
                });
            }
            weatherListHourly.AddRange(listOfMissingDatesHourly);
            weatherListHourly = weatherListHourly.OrderBy(x => x.date).ToList();

            List<Weather> listOfMissingDatesDaily = new List<Weather>();
            double averageWindSpeedDaily = weatherListDaily.Select(x => x.windSpeed).Average();
            for (DateTime start = startDate; start < weatherListDaily.First().date; start = start.AddDays(1))
            {
                listOfMissingDatesDaily.Add(new Weather()
                {
                    windSpeed = averageWindSpeed,
                    date = start,
                    Area = weatherListHourly.First().Area
                });
            }
            weatherListDaily.AddRange(listOfMissingDatesDaily);
            weatherListDaily = weatherListDaily.OrderBy(x => x.date).ToList();
        }

        void GetChartWindow()
        {
            if (forecast.Daily != null || forecast.Hourly != null)
            {
                if (forecast.Daily.Data != null && forecast.Hourly.Data != null)
                {
                    previousSelectedLocation = selectedLocation;
                    prevStartDate = startDate;
                    prevEndDate = endDate;
                    chartWindow = new ChartWindow();
                    chartWindow.Show();
                }
            }
            else MessageBox.Show("No data available for this period of time");
        }

        void ClearPinsOnMap()
        {
            if (bingMap.Children.Count > 0)
                bingMap.Children.RemoveAt(0);
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearPinsOnMap();
            e.Handled = true;
            Point mousePos = e.GetPosition(this);
            Location pinLocation = bingMap.ViewportPointToLocation(mousePos);
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            selectedLocation = pinLocation;
            bingMap.Children.Add(pin);
        }


        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (searchLocationTB.Text != string.Empty)
            {
                string response = new AsynchronousClient().SendMessage(11000, "get_location:" + searchLocationTB.Text);
                string[] cityArray = response.Split(';');
                City foundCity = new City()
                {
                    Name = cityArray[0],
                    Latitude = double.Parse(cityArray[1]),
                    Longitude = double.Parse(cityArray[2])
                };
                //new BingMap().GeocodeByAddress(searchLocationTB.Text);
                searchLocationTB.Text = foundCity.Name;
                ClearPinsOnMap();
                Pushpin pin = new Pushpin();
                pin.Location = new Location(foundCity.Latitude, foundCity.Longitude);
                selectedLocation = pin.Location;
                bingMap.Children.Add(pin);
                bingMap.Center = selectedLocation;
            }
        }

        public bool CheckDates()
        {
            if (startDateDP.SelectedDate == null)
            {
                MessageBox.Show("Start Date must be picked");
                return false;
            }

            else if (startDateDP.SelectedDate != null && endDateDP.SelectedDate == null)
            {
                startDate = (DateTime)startDateDP.SelectedDate;
                endDate = startDate;
                return true;
            }

            else
            {
                startDate = (DateTime)startDateDP.SelectedDate;
                endDate = (DateTime)endDateDP.SelectedDate;
                if (startDate > endDate)
                {
                    startDate = endDate;
                    MessageBox.Show("Start Date must be earlier than End Date");
                }
                return true;
            }
        }
        public bool CheckSelectedRegionForBingMap()
        {
            if (selectedLocation == null)
            {
                MessageBox.Show("Please select region via search button or double clicking on the map");
                return false;
            }
            else return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (bingMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.RoadMode")
            {
                bingMap.Mode = new AerialMode(true);
            }
            else if (bingMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.AerialMode")
            {
                bingMap.Mode = new RoadMode();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CompareGeneratorsStats window = new CompareGeneratorsStats();
            window.Show();
        }
    }
}
