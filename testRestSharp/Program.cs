using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRestSharp
{
    public class App
    {
        public static void DoRequest()
        {
            RestClient client = new RestClient("https://api.github.com");
            RestRequest request = new RestRequest("/users/achiu", Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
