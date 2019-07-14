using Derivco_API_tests_Doroshchuk.DataEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Derivco_API_tests_Doroshchuk.Tests
{
    [TestFixture]
    public class TestsCompanyAPI : BaseTestsAPI
    {
        [Test]
        public void CreateCompanyTest()
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.POST);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = "TestCompany1"
                });
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetCompaniesTest()
        {
            var expectedCompanyList = new List<Company>()
            {
                new Company()
                {
                    Id = "1",
                    Name = "TestCompany"
                },
                new Company()
                {
                    Id = "2",
                    Name = "TestCompany1"
                }
            };
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            var actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());
            Assert.True(actualCompanyList.SequenceEqual(expectedCompanyList));
        }
    }
}
