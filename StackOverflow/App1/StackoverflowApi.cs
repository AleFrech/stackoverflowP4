using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;

namespace App1
{
    class StackoverflowApi
    {
        private RestClient cliente;
        public StackoverflowApi()
        {
            cliente = new RestClient()
            {
               BaseUrl = new Uri("http://localhost:16470/")
            };
        }

        public IEnumerable<QuestionListModel> GetQuestionListModels()
        {
            RestRequest request = new RestRequest{Resource = "api/QuestionApi"};
            var result = cliente.Execute(request);
            RestSharp.Portable.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var list = deserial.Deserialize<IEnumerable<QuestionListModel>>(result.Result);
            return list;
        }
    }
}
