using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.API
{
    public class AccountLoginApiController : ApiController
    {
        public HttpResponseMessage PostLogin(AccountLoginModel model)
        {
            if (!model.Email.IsNullOrWhiteSpace() && !model.Password.IsNullOrWhiteSpace())
            {
                var context = new StackOverflowContext();
                string pass = EncruptDecrypt.Encrypt(model.Password);
                var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == pass);
                if (account != null)
                {
                    HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);
                    var session = new Session();
                    session.LoggedId = account.Id;
                    session.token = EncruptDecrypt.Encrypt(model.Email);
                    context.Sessions.Add(session);
                    //WP windowphone.set token=context.session.firstordefault
                    context.SaveChanges();
                    
                    return response;
                }
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
        }
    }
}