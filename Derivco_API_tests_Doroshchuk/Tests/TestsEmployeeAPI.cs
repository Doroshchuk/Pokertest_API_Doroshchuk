using Derivco_API_tests_Doroshchuk.DataEntity;
using Derivco_API_tests_Doroshchuk.Resource;
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
    public class TestsEmployeeAPI : BaseTestsAPI
    {
        private EmployeeResource _resource;

        [OneTimeSetUp]
        public void RunBeforeTests()
        {
            _resource = new EmployeeResource(_token);
        }

        [TearDown]
        public void RunAfterEachTest()
        {
            _resource.DeleteAll();
        }

        [TestCase("TestEmployee1")]
        public void CreateEmployee_StatusCodeIsOK_ValidEmployeeNameIsGiven(string employeeName)
        {
            IRestResponse response = _resource.Create(employeeName);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [TestCase("TestEmployee2")]
        public void CreateEmployee_EmployeeIsAdded_ValidEmployeeNameIsGiven(string employeeName)
        {
            _resource.Create(employeeName);
            List<Employee> actualEmployeeList = _resource.GetEmployees();

            Assert.IsTrue(actualEmployeeList.Exists(x => x.Name == employeeName));
        }

        [TestCase("TestEmployee3")]
        public void CreateEmployee_StatusCodeIsBadRequest_ExistedEmployeeNameIsGiven(string employeeName)
        {
            _resource.Create(employeeName);
            IRestResponse response = _resource.Create(employeeName);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("TestEmployee3")]
        public void CreateEmployee_EmployeeIsAdddedOnce_ExistedEmployeeNameIsGiven(string employeeName)
        {
            _resource.Create(employeeName);
            _resource.Create(employeeName);
            List<Employee> actualEmployeeList = _resource.GetEmployees();

            Assert.AreEqual(actualEmployeeList.FindAll(x => x.Name == employeeName).Count(), 1);
        }

        [Test]
        public void GetAllEmployees_StatusCodeIsOK()
        {
            IRestResponse response = _resource.GetAll();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetAllEmployees_IsTrue_ActualEmployeeListEqualsExpectedEmployeeList()
        {
            var employeeList = new List<Employee>()
            {
                new Employee()
                {
                    Name = "TestEmployee1"
                },
                new Employee()
                {
                    Name = "TestEmployee1"
                },
                new Employee()
                {
                    Name = "TestEmployee1"
                }
            };
            employeeList.ForEach(employee => _resource.Create(employee.Name));
            employeeList.ForEach(employee => employee.Id = _resource.GetIdByName(employee.Name));
            List<Employee> actualEmployeeList = _resource.GetEmployees();

            Assert.True(actualEmployeeList.Count == employeeList.Count
                            && actualEmployeeList.All(employeeList.Contains));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void GetEmployeeById_StatusCodeIsNotFound_InvalidIdIsGiven(int employeeId)
        {
            IRestResponse response = _resource.GetById(employeeId); ;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GetEmployeeById_StatusCodeIsOK_ValidIdIsGiven()
        {
            string employeeName = "TestEmployee";
            _resource.Create(employeeName);
            int employeeId = _resource.GetIdByName(employeeName);
            IRestResponse response = _resource.GetById(employeeId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetEmployeeyById_EmployeeNameAsExpected_ValidIdIsGiven()
        {
            string employeeName = "TestEmployee";
            _resource.Create(employeeName);
            int employeeId = _resource.GetIdByName(employeeName);
            IRestResponse response = _resource.GetById(employeeId);
            Employee employeeResponse =
                        new JsonDeserializer().
                                Deserialize<Employee>(response);

            Assert.AreEqual(employeeName, employeeResponse.Name);
        }
    }
}
