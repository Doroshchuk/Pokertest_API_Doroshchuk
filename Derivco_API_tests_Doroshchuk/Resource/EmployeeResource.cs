﻿using Derivco_API_tests_Doroshchuk.DataEntity;
using Derivco_API_tests_Doroshchuk.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco_API_tests_Doroshchuk.Resource
{
    public class EmployeeResource : Resource
    {
        public EmployeeResource(string token) : base(token, "/employees")
        {
        }

        // TODO : replace with GetIdByName() api method when it will be ready
        public int GetCompanyIdByName(string companyName)
        {
            var companies = GetEmployees();
            return companies.First(x => x.Name == companyName).Id;
        }

        public List<Employee> GetEmployees()
        {
            var response = GetAll();
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            return JsonConvert.DeserializeObject<List<Employee>>(jsonResponse.ToString());
        }

        // TODO : replace with DeleteAll() api method when it will be ready
        public void DeleteAll()
        {
            var companies = GetEmployees();
            companies.ForEach(company => DeleteById(company.Id));
        }
    }
}
