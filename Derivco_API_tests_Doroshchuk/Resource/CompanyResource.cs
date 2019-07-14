using Derivco_API_tests_Doroshchuk.DataEntity;
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
    public class CompanyResource
    {
        private string _baseURL = $"{Constant.BaseURL}/api/automation";
        private string _resourcePath = "/companies";
        private RestClient _client;
        private string _token;

        public CompanyResource(string token)
        {
            _client = new RestClient(_baseURL);
            _token = token;
        }

        public IRestResponse Create(string companyName)
        {
            RestRequest request = new RestRequest(_resourcePath, Method.POST);

            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = companyName
                });
            return _client.Execute(request);
        }

        public IRestResponse GetAll()
        {
            RestRequest request = new RestRequest(_resourcePath, Method.GET);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        public IRestResponse GetById(int companyId)
        {
            RestRequest request = new RestRequest($"{_resourcePath}/id/{companyId}", Method.GET);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        public IRestResponse DeleteById(int companyId)
        {
            RestRequest request = new RestRequest($"{_resourcePath}/id/{companyId}", Method.DELETE);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        // TODO : replace with GetIdByName() api method when it will be ready
        public int GetCompanyIdByName(string companyName)
        {
            var companies = GetCompanies();
            return companies.First(x => x.Name == companyName).Id;
        }

        public List<Company> GetCompanies()
        {
            var response = GetAll();
            JArray jsonResponse = (JArray)JsonConvert.DeserializeObject(response.Content);
            return JsonConvert.DeserializeObject<List<Company>>(jsonResponse.ToString());
        }

        // TODO : replace with DeleteAll() api method when it will be ready
        public void DeleteAll()
        {
            var companies = GetCompanies();
            companies.ForEach(company => DeleteById(company.Id));
        }
    }
}
