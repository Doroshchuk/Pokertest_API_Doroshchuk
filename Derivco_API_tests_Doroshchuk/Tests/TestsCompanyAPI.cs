using NUnit.Framework;
using RestSharp;
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
            var res = response.StatusCode;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
