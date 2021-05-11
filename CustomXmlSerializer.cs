using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Infotecs.ConsoleApp3
{
    public class CustomXmlSerializer
    {
        public string Serialize<T>(T sourceObject)
        {
            if (sourceObject == null)
            {
                return string.Empty;
            }

            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true });

            xmlserializer.Serialize(writer, sourceObject);
            return stringWriter.ToString();
        }

        public bool XMLValidate(string schemaUri, string xmlPath)
        {
            var errors = new StringBuilder();
            XmlReaderSettings ordersSettings = new XmlReaderSettings();

            ordersSettings.ValidationType = ValidationType.Schema;
            ordersSettings.Schemas.Add(null, schemaUri);
            ordersSettings.ValidationEventHandler += (sender, e) =>
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    errors.Append("WARNING: ");
                    errors.Append(e.Message + Environment.NewLine);
                }
                if (e.Severity == XmlSeverityType.Error)
                {
                    errors.Append("ERROR: ");
                    errors.Append(e.Message + Environment.NewLine);
                }
            };

            if (errors.Length > 0)
            {
                Console.WriteLine(errors);
                return false;
            }

            XmlReader reader = XmlReader.Create(new StreamReader(xmlPath), ordersSettings);
            while (reader.Read());

            return true;
        }
    }
}
