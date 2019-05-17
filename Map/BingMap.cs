using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace WindEnergy
{
    public class BingMap
    {
        private string apiKey = ConfigurationManager.AppSettings["Mapapi"];
        public BingMap()
        {

        }
        // Geocode an address and return a latitude and longitude
        public City GeocodeByAddress(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + apiKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);
            City city = new City();
            return city.Deserialize(geocodeResponse);
        }

        public City GeocodeByPoint(Location point)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + point.Latitude.ToString().Replace(',', '.') + ", " + point.Longitude.ToString().Replace(',', '.') + "?o=xml&key=" + apiKey;
            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);
            //if(geocodeResponse)
            City city = new City();
            return city.Deserialize(geocodeResponse);
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(response.GetResponseStream());
                    return xmlDoc;
                }
            }
            catch
            {
                MessageBox.Show("Location was not found");
                XmlDocument xmlDocq = new XmlDocument();
                return xmlDocq;
            }
            //XmlDocument xmlDocq = new XmlDocument();
            //return xmlDocq;
        }
    }
}
