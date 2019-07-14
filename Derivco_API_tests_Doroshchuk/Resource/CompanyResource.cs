using Derivco_API_tests_Doroshchuk.DataEntity;
using Derivco_API_tests_Doroshchuk.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivco_API_tests_Doroshchuk.Resource
{
    public class CompanyResource : Resource
    {
        public CompanyResource(string token) : base(token, "/companies")
        {
        }

        // TODO : replace with GetIdByName() api method when it will be ready
        public override int GetIdByName(string name)
        {
            var companies = GetCompanies();
            return companies.First(x => x.Name == name).Id;
        }

        public List<Company> GetCompanies()
        {
            var response = GetAll();
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            return JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());
        }

        // TODO : replace with DeleteAll() api method when it will be ready
        public override void DeleteAll()
        {
            var companies = GetCompanies();
            companies.ForEach(company => DeleteById(company.Id));
        }
    }
}
