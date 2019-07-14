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
        [TestCase("TestCompany1")]
        public void CreateCompany_StatusCodeIsOK_ValidCompanyNameIsGiven(string companyName)
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.POST);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = companyName
                });
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TestCase("TestCompany2")]
        public void CreateCompany_StatusCodeIsBadRequest_ExistedCompanyNameIsGiven(string companyName)
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.POST);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = companyName
                });
            client.Execute(request);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetAllCompanies_StatusCodeIsOK()
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetAllCompanies_IsTrue_ActualCompanyListIsEqualToExpectedCompanyList()
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

        [TestCase(1, HttpStatusCode.OK, TestName = "Verify 'OK' status code for company with id 1")]
        [TestCase(0, HttpStatusCode.NotFound, TestName = "Verify 'Not Found' status code for company with id 0")]
        [TestCase(-5, HttpStatusCode.NotFound, TestName = "Verify 'Not Found' status code for company with id -5")]
        public void GetCompanyById_VerifyStatusCode(int companyId, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{companyId}", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [Test]
        public void GetCompanyById_ActualCompanyNameAsExpected_ValidIdIsGiven()
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{1}", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);
            Company companyResponse =
                        new JsonDeserializer().
                                Deserialize<Company>(response);

            // assert
            Assert.AreEqual("TestCompany", companyResponse.Name);
        }

        [TestCase(1, HttpStatusCode.OK, TestName = "Verify 'OK' status code after deleting company with id 1")]
        [TestCase(2, HttpStatusCode.OK, TestName = "Verify 'OK' status code after deleting company with id 2")]
        [TestCase(3, HttpStatusCode.OK, TestName = "Verify 'OK' status code after deleting company with id 3")]
        public void DeleteCompanyById_VerifyStatusCode(int companyId, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{companyId}", Method.DELETE);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [TestCase(1, TestName = "Verify company with id 1 is deleted")]
        public void DeleteCompanyById_VerifyCompanyIsDeleted(int companyId)
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest deleteRequest = new RestRequest($"/companies/id/{companyId}", Method.DELETE);
            RestRequest getRequest = new RestRequest($"/companies/id/{companyId}", Method.GET);

            // act
            deleteRequest.AddHeader("authorization", "Bearer " + _token);
            getRequest.AddHeader("authorization", "Bearer " + _token);
            client.Execute(deleteRequest);
            IRestResponse response = client.Execute(getRequest);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
