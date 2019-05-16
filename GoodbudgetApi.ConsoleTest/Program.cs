using System;
using System.Linq;
using RestSharp;

namespace GoodbudgetApi.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GoodbudgetHousehold household = GoodbudgetHousehold.Login("ikahl", "y``u14N3Pin?3-QH");

            var accounts = household.GetAccounts();

            Console.WriteLine(string.Join(Environment.NewLine, accounts.Select(x => x.Name + ": " + x.CurrentBalance)));

            household.Logout();
        }
    }
}
