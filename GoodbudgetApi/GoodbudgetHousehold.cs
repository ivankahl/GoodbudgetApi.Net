using System;
using System.Collections.Generic;
using System.Linq;
using GoodbudgetApi.Exceptions;
using GoodbudgetApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace GoodbudgetApi
{
    public class GoodbudgetHousehold : IDisposable
    {
        /// <summary>
        /// Stores the session id for the current session
        /// </summary>
        private string _sessionId;

        public ICollection<GoodbudgetAccount> GetAccounts() 
        {
            var client = RestSharpFactory.GetRestClient();

            var request = RestSharpFactory.GetRestRequest("/api/accounts");
            request.AddCookie("GBSESS", _sessionId);

            var response = client.Execute(request);

            var obj = JArray.Parse(response.Content);

            // To avoid creating a ton of classes to deserialize the result, I use a JObject...
            return JsonConvert.DeserializeObject<ICollection<GoodbudgetAccount>>(
                (
                    (JObject)(
                        (JArray)(
                            (JObject)obj[0]
                        )["nodes"]
                    ).FirstOrDefault(x => x["Name"].Value<string>() == "Checking, Savings, Cash")
                )["nodes"].ToString());
        }

        /// <summary>
        /// Logs out of the current session for the household.
        /// </summary>
        public void Logout() 
        {
            var client = RestSharpFactory.GetRestClient();

            var request = RestSharpFactory.GetRestRequest("/logout");
            request.AddCookie("GBSESS", _sessionId);

            client.Execute(request);
        }

        /// <summary>
        /// Attempts to log into Goodbudget with the given credentials and, if successful
        /// returns the new <see cref="GoodbudgetHousehold"/> object that has been authenticated.
        /// </summary>
        /// <param name="username">The username to use to log in</param>
        /// <param name="password">The password to use to log in</param>
        /// <returns>The authenticated <see cref="GoodbudgetHousehold"/> object</returns>
        public static GoodbudgetHousehold Login(string username, string password)
        {
            var client = RestSharpFactory.GetRestClient();
            client.FollowRedirects = false;

            // We will attempt to send a login POST request
            var request = RestSharpFactory.GetRestRequest("/login_check", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"_username={username}&_password={password}",
                ParameterType.RequestBody);

            var response = client.Execute(request);
            
            // If we were redirected to the login page, the credentials were wrong so we will
            // throw an exception
            if (response.Headers.First(x => x.Name.ToLower() == "location").Value.ToString().ToLower() == "https://goodbudget.com/login")
                throw new AuthenticationException("Could not authenticate with Goodbudget.  Please ensure you have entered the correct credentials.");

            // Otherwise the credentials were correct and our current session id was authenticated
            // so we can use it to access the API now
            return new GoodbudgetHousehold() 
            {
                _sessionId = response.Cookies.First(x => x.Name.ToLower() == "gbsess").Value.ToString()
            };
        }

        /// <summary>
        /// Logs the user out and disposes of the object
        /// </summary>
        public void Dispose()
        {
            Logout();
        }
    }
}
