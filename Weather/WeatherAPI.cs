using DarkSky.Models;
using DarkSky.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static DarkSky.Services.DarkSkyService;

namespace WindEnergy
{
    public class WeatherAPI
    {
        private string apiKey = ConfigurationManager.AppSettings["Weatherapi"];
        ZipHttpClient client = new ZipHttpClient("https://api.darksky.net/");
        OptionalParameters parameters = new OptionalParameters();
        DarkSkyEnumJsonConverter converter = new DarkSkyEnumJsonConverter();
        public DarkSkyService darkSky;
        public WeatherAPI()
        {
            darkSky = new DarkSkyService(apiKey, client);
        }

        public List<Weather> GetWindSpeed(List<DataPoint> data, City area)
        {
            List<Weather> wind = new List<Weather>();
            foreach (var item in data)
            {
                wind.Add(new Weather()
                {
                    windSpeed = item.WindSpeed == null ? 0 : (double)item.WindSpeed,
                    date = item.DateTime.DateTime,
                    Area = area
                });
            }
            return wind;
        }

        public List<Weather> GetWindSpeed(List<DataPoint> data, City area, double windSpeed)
        {
            List<Weather> wind = new List<Weather>();
            foreach (var item in data)
            {
                wind.Add(new Weather()
                {
                    windSpeed = windSpeed,
                    date = item.DateTime.DateTime,
                    Area = area
                });
            }
            return wind;
        }
    }
}
