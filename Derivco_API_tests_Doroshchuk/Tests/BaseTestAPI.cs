using Derivco_API_tests_Doroshchuk.Helpers;
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
    public class BaseTestAPI
    {
        protected string _token;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            RestClient client = new RestClient($"{Constant.BaseURL}/token");
            RestRequest request = new RestRequest(Method.POST);

            request.AddParameter("username", "testName");
            request.AddParameter("password", "test");
            request.AddParameter("grant_type", "password");
            IRestResponse response = client.Execute(request);

            dynamic api = JObject.Parse(response.Content);
            _token = api.access_token;
            // TODO : check for token is not accepted
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {

        }
    }
}
