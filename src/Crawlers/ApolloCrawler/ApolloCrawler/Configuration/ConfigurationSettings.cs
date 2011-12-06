using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ApolloCrawler.Configuration
{
    public class ConfigurationSettings
    {
        [XmlElement(Order = 1)]
        public string SolrUrl { get; set; }

        [XmlElement(Order = 2)]
        public Project[] Projects { get; set; }
        
        public static ConfigurationSettings Load()
        {
            string configFile = ConfigurationManager.AppSettings["ConfigFile"];
            string configXml = File.ReadAllText(configFile);

            return ConfigurationSettings.Deserialize(configXml);
        }

        public string Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConfigurationSettings));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            //get rid of the first xml line
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true });

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); //gets rid of the xmlns:xsd crap

            xs.Serialize(xw, this, ns);

            xw.Close();
            sw.Close();

            return sb.ToString();

        }

       
        public static ConfigurationSettings Deserialize(string xml)
        {

            ConfigurationSettings returnVal;
            XmlSerializer serial = new XmlSerializer(typeof(ConfigurationSettings));
            StringReader reader = new StringReader(xml);
            returnVal = (ConfigurationSettings)serial.Deserialize(reader);

            return returnVal;

        }
    }
}
