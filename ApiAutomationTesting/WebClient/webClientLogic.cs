
using Rest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Tiny.RestClient;
using Tiny.RestClient.Json;

namespace ApiAutomationTesting.WebClient
{
    public class webClientLogic
    {
        
        public async void testtiny(string uri, string user, string pass)
        {
            RestClient client = new RestClient("http://www.example.com/");
            RestRequest request = new RestRequest("login", Method.POST);
            request.AddHeader("Accept", "application/json");
            var body = new
            {
                Host = "host_environment",
                Username = "UserID",
                Password = "Password"
            };
            request.AddJsonBody(body);

            var response = client.Execute(request).Content;
        }


        public DataSet GetParamsBuildTest(string uri, string user, string pass)
        {
            DataSet ds = new DataSet();
            webClient p = new webClient();
            

            Task.Run(async () => { ds = await p.GetEcecutionTfsTestCase(uri, user, pass); }).GetAwaiter().GetResult();
            return ds;
        }

    }
}
