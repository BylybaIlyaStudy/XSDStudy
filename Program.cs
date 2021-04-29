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
        private StringBuilder errors = new StringBuilder();

        private readonly CustomerType customer = new CustomerType
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
        private readonly OrderType order = new OrderType
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

        public static void Main(string[] args)
        {
            var program = new Program();

            Root root = new Root
            {
                Customers = new CustomerType[] { program.customer },
                Orders = new OrderType[] { program.order }
            };

            string xml = program.Serialize(root);
            File.WriteAllText($"{args[0]}Output.xml", xml);

            program.XMLValidate(args[0]);

            if (program.errors.Length != 0)
            {
                Console.WriteLine("XML is INVALID");
                Console.WriteLine(program.errors);

                return;
            }

            Console.WriteLine("XML is VALID.");
        }

        private string Serialize<T>(T sourceObject)
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

        private void XMLValidate(string path)
        {
            XmlReaderSettings ordersSettings = new XmlReaderSettings();

            ordersSettings.ValidationType = ValidationType.Schema;
            ordersSettings.Schemas.Add(null, $"{path}XMLSchema1.xsd");
            ordersSettings.ValidationEventHandler += new ValidationEventHandler(OrdersSettingsValidationEventHandler);

            XmlReader reader = XmlReader.Create(new StreamReader($"{path}Output.xml"), ordersSettings);
            while (reader.Read()) ;
        }

        private void OrdersSettingsValidationEventHandler(object sender, ValidationEventArgs e)
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
        }
    }
}
