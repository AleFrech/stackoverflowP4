using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace StackOverflow.Domain
{
    public static class EmailVerifcations
    {
        public static IRestResponse SendForgotPasswordMessage(string destination, string code,Guid accId)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v2"),
                Authenticator = new HttpBasicAuthenticator("api","key-8tw489mxfegaqewx93in2xo449q5p3l0")
            };
            var request = new RestRequest();
            request.AddParameter("domain","app5dcaf6d377cc4ddcb696b827eabcb975.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "StackOverflow_verify@proga4.com");
            String email = destination;
            request.AddParameter("to", email);
            request.AddParameter("subject", "Password Recovery ");
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)+ "/Account/VerifyCode/" + accId;
            if (verifyUrl.Contains("localhost"))
            {
                request.AddParameter("text","Enter the verification code: " + code + " In the folowing link to change your Password : " + verifyUrl);
                request.Method = Method.POST;
                return client.Execute(request);
            }
            verifyUrl = "http://stackoverflowp4.apphb.com/Account/VerifyCode/" + accId;
            request.AddParameter("text","Enter the verification code: " + code + " In the folowing link to change your Password : " + verifyUrl);
            request.Method = Method.POST;
            return client.Execute(request);
        } 

    }
}