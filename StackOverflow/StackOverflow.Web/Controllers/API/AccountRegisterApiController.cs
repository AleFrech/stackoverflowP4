using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.API
{
    public class AccountRegisterApiController : ApiController
    {
        public HttpResponseMessage PostRegister(AccountRegisterModel model)
        {

            if (model!=null && !model.Email.IsNullOrWhiteSpace() && !model.Name.IsNullOrWhiteSpace()&&!model.Password.IsNullOrWhiteSpace() && model.ConfirmPassword.Equals(model.Password))
            {
                var context = new StackOverflowContext();
                var account = Mapper.Map<AccountRegisterModel, Account>(model);
                account.Password = EncruptDecrypt.Encrypt(model.Password);
                account.VerifyEmail = true;
                account.CreationDate = (DateTime.Now).ToString();
                account.ProfileViews = 0;
                account.LasTimeSeen = DateTime.Now.ToString();
                context.Accounts.Add(account);
                context.SaveChanges();
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);

                return response;
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
        }
      
        

    }
}