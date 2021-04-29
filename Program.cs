using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Infotecs.ConsoleApp3
{
    class Program
    {
        private static string errors = String.Empty;
        private static string path = "C:/Users/bylyba.ilia/source/repos/ConsoleApp3/ConsoleApp3/";


        static void Main(string[] args)
        {
            var customer = new CustomerType
            {
                CustomerID = "GREAL",
                CompanyName = "Great Lakes Food Market",
                ContactName = "Howard Snyder",
                ContactTitle = "Marketing Manager",
                Phone = "(503) 555-7555",
                Fax = "(503) 555-2376",
                FullAddress = new AddressType
                {
                    Address = "2732 Baker Blvd.",
                    City = "Eugene",
                    Region = "OR",
                    PostalCode = "97403",
                    Country = "USA"
                }
            };

            var order = new OrderType
            {
                CustomerID = "GREAL",
                EmployeeID = "6",
                OrderDate = DateTime.Parse("1997-05-06T00:00:00"),
                RequiredDate = DateTime.Parse("1997-05-20T00:00:00"),
                ShipInfo = new ShipInfoType
                {
                    ShippedDate = DateTime.Parse("1997-05-09T00:00:00"),
                    ShipVia = "2",
                    Freight = 3.35M,
                    ShipName = "Great Lakes Food Market",
                    ShipAddress = "2732 Baker Blvd.",
                    ShipCity = "Eugene",
                    ShipRegion = "OR",
                    ShipPostalCode = "97403",
                    ShipCountry = "USA"
                }
            };

            var root = new Root
            {
                Customers = new CustomerType[] { customer },
                Orders = new OrderType[] { order }
            };

            var xml = Serialize(root);
            Console.WriteLine(xml);
            File.WriteAllText(path + "Output.xml", xml, Encoding.Unicode);

            XmlReaderSettings ordersSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            ordersSettings.Schemas.Add(null, path + "XMLSchema1.xsd");
            ordersSettings.ValidationEventHandler += new ValidationEventHandler(ordersSettingsValidationEventHandler);

            XmlReader reader = XmlReader.Create(new StreamReader(path + "Output.xml"), ordersSettings);
            while (reader.Read()) ;

            if (errors.Length == 0)
            {
                Console.WriteLine("\nXML is VALID.");
            }
            else
            {
                Console.WriteLine("\nXML is INVALID");
                Console.WriteLine(errors);
            }
        }

        private static string Serialize<TType>(TType sourceObject)
        {
            if (sourceObject == null)
            {
                return string.Empty;
            }

            var xmlserializer = new XmlSerializer(typeof(TType));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true }))
            {
                xmlserializer.Serialize(writer, sourceObject);
                return stringWriter.ToString();
            }
        }

        static void ordersSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                errors += "WARNING: ";
                errors += e.Message + "\n";
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                errors += "ERROR: ";
                errors += e.Message + "\n";
            }
        }
    }
}
