using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StackOverflow.Domain
{
   public static  class EmailVerifcations
    {
        public static IRestResponse SendSimpleMessage(string destination, string code)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v2"),
                Authenticator = new HttpBasicAuthenticator("api",
                    "key-8tw489mxfegaqewx93in2xo449q5p3l0")
            };
            var request = new RestRequest();
            request.AddParameter("domain",
                "app5dcaf6d377cc4ddcb696b827eabcb975.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from","StackOverflow_verify@proga4.com");
            String email =destination;
            request.AddParameter("to", email);
            request.AddParameter("subject", "Password Code");
            request.AddParameter("text", code);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}
