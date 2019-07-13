using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco_API_tests_Doroshchuk.Tests
{
    [SetUpFixture]
    public class BaseTestsAPI
    {
        protected string _token;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/token");
            RestRequest request = new RestRequest(Method.POST);

            request.AddParameter("username", "testName");
            request.AddParameter("password", "test");
            request.AddParameter("grant_type", "password");
            IRestResponse response = client.Execute(request);

            dynamic api = JObject.Parse(response.Content);
            var access_token = api.access_token;
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {

        }
    }
}
