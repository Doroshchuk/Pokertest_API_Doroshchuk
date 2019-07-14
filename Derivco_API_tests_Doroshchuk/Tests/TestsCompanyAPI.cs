﻿using Derivco_API_tests_Doroshchuk.DataEntity;
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
        public void CreateCompany_CompanyIsAdded_ValidCompanyNameIsGiven(string companyName)
        {
            _resource.Create(companyName);
            List<Company> actualCompanyList = _resource.GetCompanies();

            Assert.IsTrue(actualCompanyList.Exists(x => x.Name == companyName));
        }

        [TestCase("TestCompany3")]
        public void CreateCompany_StatusCodeIsBadRequest_ExistedCompanyNameIsGiven(string companyName)
        {
            _resource.Create(companyName);
            IRestResponse response = _resource.Create(companyName);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("TestCompany3")]
        public void CreateCompany_CompanyIsAdddedOnce_ExistedCompanyNameIsGiven(string companyName)
        {
            _resource.Create(companyName);
            _resource.Create(companyName);
            List<Company> actualCompanyList = _resource.GetCompanies();

            Assert.AreEqual(actualCompanyList.FindAll(x => x.Name == companyName).Count(), 1);
        }

        [Test]
        public void GetAllCompanies_StatusCodeIsOK()
        {
            IRestResponse response = _resource.GetAll();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetAllCompanies_IsTrue_ActualCompanyListEqualsExpectedCompanyList()
        {
            var companyList = new List<Company>()
            {
                new Company()
                {
                    Name = "TestCompany1"
                },
                new Company()
                {
                    Name = "TestCompany2"
                },
                new Company()
                {
                    Name = "TestCompany3"
                }
            };
            companyList.ForEach(company => _resource.Create(company.Name));
            companyList.ForEach(company => company.Id = _resource.GetCompanyIdByName(company.Name));
            List<Company> actualCompanyList = _resource.GetCompanies();

            Assert.True(actualCompanyList.Count == companyList.Count
                            && actualCompanyList.All(companyList.Contains));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void GetCompanyById_StatusCodeIsNotFound_InvalidIdIsGiven(int companyId)
        {
            IRestResponse response = _resource.GetById(companyId); ;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GetCompanyById_StatusCodeIsOK_ValidIdIsGiven()
        {
            string companyName = "TestCompany";
            _resource.Create(companyName);
            int companyId = _resource.GetCompanyIdByName(companyName);
            IRestResponse response = _resource.GetById(companyId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetCompanyById_CompanyNameAsExpected_ValidIdIsGiven()
        {
            string companyName = "TestCompany";
            _resource.Create(companyName);
            int companyId = _resource.GetCompanyIdByName(companyName);
            IRestResponse response = _resource.GetById(companyId);
            Company companyResponse =
                        new JsonDeserializer().
                                Deserialize<Company>(response);

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
