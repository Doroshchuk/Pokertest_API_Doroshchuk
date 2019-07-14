using Derivco_API_tests_Doroshchuk.Resource;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
