﻿using Derivco_API_tests_Doroshchuk.DataEntity;
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
        public void CreateCompany_StatusCodeAsExpected()
        {
            // arrange
            RestClient client = new RestClient("https://mobilewebserver9-pokertest8ext.installprogram.eu/TestApi/api/automation");
            RestRequest request = new RestRequest("/companies", Method.POST);

            // act
            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = "TestCompany"
                });
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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
    }
}
