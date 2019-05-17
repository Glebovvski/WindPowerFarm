using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WindEnergy
{
    [Serializable()]
    public class City
    {
        [System.Xml.Serialization.XmlElement("Name")]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlElement("Latitude")]
        public double Latitude { get; set; }
        [System.Xml.Serialization.XmlElement("Longitude")]
        public double Longitude { get; set; }

        public City Deserialize(XmlDocument document)
        {
            if (document.ChildNodes.Count > 0)
            {
                City city = new City();
                Response response;
                XmlSerializer serializer = new XmlSerializer(typeof(Response));
                using (XmlReader reader = new XmlNodeReader(document))
                {
                    response = (Response)serializer.Deserialize(reader);
                }
                city.Name = response.ResourceSets.ResourceSet.Resources.Select(x => x.Address.FormattedAddress).FirstOrDefault();
                city.Latitude = (double)response.ResourceSets.ResourceSet.Resources.Select(x => x.GeocodePoint.Latitude).FirstOrDefault();
                city.Longitude = (double)response.ResourceSets.ResourceSet.Resources.Select(x => x.GeocodePoint.Longitude).FirstOrDefault();
                return city;
            }
            else return new City();
        }
    }
}
