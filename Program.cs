using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Infotecs.ConsoleApp3
{
    class Program
    {
        private static string errors = string.Empty;
        private static readonly string path = "C:/Users/bylyba.ilia/source/repos/ConsoleApp3/ConsoleApp3/";

        private static readonly CustomerType customer = new CustomerType
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
        private static readonly OrderType order = new OrderType
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

        public static void Main()
        {
            Root root = new Root
            {
                Customers = new CustomerType[] { customer },
                Orders = new OrderType[] { order }
            };

            string xml = Serialize(root);
            File.WriteAllText($"{path}Output.xml", xml);

            XmlReaderSettings ordersSettings = new XmlReaderSettings();

            ordersSettings.ValidationType = ValidationType.Schema;
            ordersSettings.Schemas.Add(null, $"{path}XMLSchema1.xsd");
            ordersSettings.ValidationEventHandler += new ValidationEventHandler(OrdersSettingsValidationEventHandler);

            XmlReader reader = XmlReader.Create(new StreamReader($"{path}Output.xml"), ordersSettings);
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

        private static string Serialize<T>(T sourceObject)
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

        private static void OrdersSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                errors += "WARNING: ";
                errors += e.Message + "\n";
            }
            if (e.Severity == XmlSeverityType.Error)
            {
                errors += "ERROR: ";
                errors += e.Message + "\n";
            }
        }
    }
}
