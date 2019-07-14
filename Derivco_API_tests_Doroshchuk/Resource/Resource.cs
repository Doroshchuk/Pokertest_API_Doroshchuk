using Derivco_API_tests_Doroshchuk.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco_API_tests_Doroshchuk.Resource
{
    public abstract class Resource
    {
        private string _baseURL = $"{Constant.BaseURL}/api/automation";
        private string _resourcePath;
        private RestClient _client;
        private string _token;

        public Resource(string token, string resourcePath)
        {
            _client = new RestClient(_baseURL);
            _token = token;
            _resourcePath = resourcePath;
        }

        public IRestResponse Create(string name)
        {
            RestRequest request = new RestRequest(_resourcePath, Method.POST);

            request.AddHeader("authorization", "Bearer " + _token);
            request.AddJsonBody(
                new
                {
                    Name = name
                });
            return _client.Execute(request);
        }

        public IRestResponse GetAll()
        {
            RestRequest request = new RestRequest(_resourcePath, Method.GET);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        public IRestResponse GetById(int id)
        {
            RestRequest request = new RestRequest($"{_resourcePath}/id/{id}", Method.GET);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        public IRestResponse DeleteById(int id)
        {
            RestRequest request = new RestRequest($"{_resourcePath}/id/{id}", Method.DELETE);

            request.AddHeader("authorization", "Bearer " + _token);
            return _client.Execute(request);
        }

        public abstract void DeleteAll();

        public abstract int GetIdByName(string name);
    }
}
