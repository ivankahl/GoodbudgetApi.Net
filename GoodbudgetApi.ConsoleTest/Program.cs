using System;
using System.Linq;
using RestSharp;

namespace GoodbudgetApi.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GoodbudgetHousehold household = GoodbudgetHousehold.Login("", "");

            var accounts = household.GetAccounts();

            Console.WriteLine(string.Join(Environment.NewLine, accounts.Select(x => x.Name + ": " + x.CurrentBalance)));

            household.Logout();
        }
    }
}
