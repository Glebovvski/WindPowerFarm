using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindEnergy
{
    public class WindGenerator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double RatedPower { get; set; }
        public double RatedWindSpeed { get; set; }
        public double Power { get; set; }
        public double WindSpeed { get; set; }
        public double MaxWindSpeed { get; set; }
        public double MinWindSpeed { get; set; }
        public bool IsWorking { get; set; }
        public string ErrorMessage { get; set; }
        public double? Radius { get; set; }
        public double? SweptArea { get; set; }
        public int? Height { get; set; }

        public decimal? Price { get; set; }

        private double KPD = 0.4;
        public double CalculatePower(double airDensity, double windSpeed)
        {
            if (SweptArea == null)
                SweptArea = Math.PI * Radius * Radius;
            if (windSpeed < MinWindSpeed)
            {
                IsWorking = false;
                ErrorMessage = "Wind Speed is below the permissable rate";
                return Power = 0;
            }
            else if (windSpeed > MaxWindSpeed)
            {
                IsWorking = false;
                ErrorMessage = "Wind Speed exceeds the permissable rate";
                return Power = 0;
            }
            else
            {
                ErrorMessage = "";
                return Power = ((((airDensity * (double)SweptArea * Math.Pow(windSpeed, 3)) / 2) )*KPD) / 1000;
            }
        }

        double WindSpeedOnSetHeight(double windSpeed)
        {
            if (Height != null)
                return WindSpeed = windSpeed * Math.Pow((double)Height / 10, 0.167);
            //else Height = 10;
            return WindSpeed = windSpeed;
        }

        public double CalculatePowerForMonth(double windSpeed, int month)
        {
            int days = DateTime.DaysInMonth(DateTime.Now.Year, month);
            WindSpeedOnSetHeight(windSpeed);
            return CalculatePower(1.2, WindSpeed) * 24 * days;
        }

        public double CalculatePowerForHour(double windSpeed)
        {
            WindSpeedOnSetHeight(windSpeed);
            return CalculatePower(1.2, WindSpeed);
        }
        public double CalculatePowerForDay(double windSpeed)
        {
            WindSpeedOnSetHeight(windSpeed);
            return CalculatePower(1.2, WindSpeed) * 24;
        }
    }
}
