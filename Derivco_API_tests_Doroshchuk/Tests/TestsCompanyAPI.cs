using Derivco_API_tests_Doroshchuk.DataEntity;
using Derivco_API_tests_Doroshchuk.Helpers;
using Derivco_API_tests_Doroshchuk.Resource;
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
        private CompanyResource _resource;

        [OneTimeSetUp]
        public void RunBeforeTests()
        {
            _resource = new CompanyResource(_token);
        }

        [TearDown]
        public void RunAfterEachTest()
        {
            _resource.DeleteAll();
        }

        [TestCase("TestCompany1")]
        public void CreateCompany_StatusCodeIsOK_ValidCompanyNameIsGiven(string companyName)
        {
            IRestResponse response = _resource.Create(companyName);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TestCase("TestCompany2")]
        [Order(2)]
        public void CreateCompany_CompanyIsAdded_ValidCompanyNameIsGiven(string companyName)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest createRequest = new RestRequest("/companies", Method.POST);
            RestRequest getAllRequest = new RestRequest("/companies", Method.GET);

            // act
            createRequest.AddHeader("authorization", "Bearer " + _token);
            getAllRequest.AddHeader("authorization", "Bearer " + _token);
            createRequest.AddJsonBody(
                new
                {
                    Name = companyName
                });
            client.Execute(createRequest);
            IRestResponse response = client.Execute(getAllRequest);
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            var actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());

            // assert
            Assert.IsTrue(actualCompanyList.Exists(x => x.Name == companyName));
        }

        [TestCase("TestCompany3")]
        [Order(3)]
        public void CreateCompany_StatusCodeIsBadRequest_ExistedCompanyNameIsGiven(string companyName)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
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

        [TestCase("TestCompany3")]
        [Order(4)]
        public void CreateCompany_CompanyIsAdddedOnce_ExistedCompanyNameIsGiven(string companyName)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest createRequest = new RestRequest("/companies", Method.POST);
            RestRequest getAllRequest = new RestRequest("/companies", Method.GET);

            // act
            createRequest.AddHeader("authorization", "Bearer " + _token);
            getAllRequest.AddHeader("authorization", "Bearer " + _token);
            createRequest.AddJsonBody(
                new
                {
                    Name = companyName
                });
            client.Execute(createRequest);
            client.Execute(createRequest);
            IRestResponse response = client.Execute(getAllRequest);
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            var actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());

            // assert
            Assert.AreEqual(actualCompanyList.FindAll(x => x.Name == companyName).Count(), 1);
        }

        [Test]
        [Order(5)]
        public void GetAllCompanies_StatusCodeIsOK()
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest request = new RestRequest("/companies", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(6)]
        public void GetAllCompanies_IsTrue_ActualCompanyListEqualsExpectedCompanyList()
        {
            var expectedCompanyList = new List<Company>()
            {
                new Company()
                {
                    Id = 14,
                    Name = "TestCompany1"
                },
                new Company()
                {
                    Id = 15,
                    Name = "TestCompany2"
                },
                new Company()
                {
                    Id = 16,
                    Name = "TestCompany3"
                }
            };
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest request = new RestRequest("/companies", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            var actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());
            Assert.True(actualCompanyList.Count == expectedCompanyList.Count 
                            && actualCompanyList.All(expectedCompanyList.Contains));
        }

        [TestCase(11, HttpStatusCode.OK, TestName = "Verify 'OK' status code for company with id 11")]
        [TestCase(0, HttpStatusCode.NotFound, TestName = "Verify 'Not Found' status code for company with id 0")]
        [TestCase(-5, HttpStatusCode.NotFound, TestName = "Verify 'Not Found' status code for company with id -5")]
        [Order(7)]
        public void GetCompanyById_VerifyStatusCode(int companyId, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{companyId}", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [TestCase(11, "TestCompany1")]
        [Order(8)]
        public void GetCompanyById_ActualCompanyNameAsExpected_ValidIdIsGiven(int companyId, string companyName)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{companyId}", Method.GET);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);
            Company companyResponse =
                        new JsonDeserializer().
                                Deserialize<Company>(response);

            // assert
            Assert.AreEqual(companyName, companyResponse.Name);
        }

        [TestCase(11, HttpStatusCode.OK)]
        [TestCase(12, HttpStatusCode.OK)]
        [TestCase(-5, HttpStatusCode.NotFound)]
        [Order(9)]
        public void DeleteCompanyById_VerifyStatusCode(int companyId, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
            RestRequest request = new RestRequest($"/companies/id/{companyId}", Method.DELETE);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [TestCase(13)]
        [Order(10)]
        public void DeleteCompanyById_VerifyCompanyIsDeleted(int companyId)
        {
            // arrange
            RestClient client = new RestClient($"{Constant.BaseURL}/api/automation");
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
