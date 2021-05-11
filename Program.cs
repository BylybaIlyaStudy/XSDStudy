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
        public static void Main(string[] args)
        {
            var serializer = new CustomXmlSerializer();
            var testDataCreator = new TestDataCreator();

            Root root = new Root
            {
                Customers = new CustomerType[] { testDataCreator.CreateCustomerType() },
                Orders = new OrderType[] { testDataCreator.CreateOrderType() }
            };

            string xml = serializer.Serialize(root);
            File.WriteAllText($"{args[0]}Output.xml", xml);

            bool xmlValid = serializer.XMLValidate($"{args[0]}XMLSchema1.xsd", $"{args[0]}Output.xml");

            if (!xmlValid)
            {
                Console.WriteLine("XML is INVALID.");
            }

            Console.WriteLine("XML is VALID.");
        }
    }
}
