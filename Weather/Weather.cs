using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindEnergy
{
    public class Weather
    {
        public WindGenerator WindGenerator { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime date { get; set; }
        public double windSpeed { get; set; }

        public City Area { get; set; }

        public Weather()
        {

        }

        public Weather(Weather weather)
        {
            this.WindGenerator = weather.WindGenerator;
            this.latitude = weather.latitude;
            this.longitude = weather.longitude;
            this.date = weather.date;
            this.windSpeed = weather.windSpeed;
            this.Area = weather.Area;
        }

        public Weather Copy()
        {
            Weather copy = new Weather(this);
            return copy;
        }
    }
}
