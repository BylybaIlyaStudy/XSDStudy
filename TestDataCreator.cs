using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infotecs.ConsoleApp3
{
    public class TestDataCreator
    {
        public CustomerType CreateCustomerType()
        {
            return new CustomerType
            {
                CustomerID = RandomString(10),
                CompanyName = RandomString(10),
                ContactName = RandomString(10),
                ContactTitle = RandomString(10),
                Phone = RandomString(10),
                Fax = RandomString(10),
                FullAddress = new AddressType
                {
                    Address = RandomString(10),
                    City = RandomString(10),
                    Region = RandomString(10),
                    PostalCode = RandomString(10),
                    Country = RandomString(10)
                }
            };
        }

        public OrderType CreateOrderType()
        {
            return new OrderType
            {
                CustomerID = RandomString(10),
                EmployeeID = RandomString(10),
                OrderDate = DateTime.Parse("1997-05-06T00:00:00"),
                RequiredDate = DateTime.Parse("1997-05-20T00:00:00"),
                ShipInfo = new ShipInfoType
                {
                    ShippedDate = DateTime.Parse("1997-05-09T00:00:00"),
                    ShipVia = RandomString(10),
                    Freight = 3.35M,
                    ShipName = RandomString(10),
                    ShipAddress = RandomString(10),
                    ShipCity = RandomString(10),
                    ShipRegion = RandomString(10),
                    ShipPostalCode = RandomString(10),
                    ShipCountry = RandomString(10)
                }
            };
        }

        private string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
