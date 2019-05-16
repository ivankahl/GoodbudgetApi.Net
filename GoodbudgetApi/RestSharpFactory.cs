using System;
using RestSharp;

namespace GoodbudgetApi
{
    public class RestSharpFactory
    {
        internal static IRestClient GetRestClient() 
        {
            return new RestClient("https://goodbudget.com/");
        }

        internal static RestRequest GetRestRequest(string path, Method method = Method.GET)
        {
            return new RestRequest(path, method)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
        }
    }
}
