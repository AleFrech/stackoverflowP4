using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers{


    public class AccountController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        public AccountController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }
        public ActionResult Register()
        {
            return View(new AccountRegisterModel());
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterModel model )
        {

            if (ModelState.IsValid)
            {
                var account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                var context = new StackOverflowContext();
                context.Accounts.Add(account);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return  View(model);

        }
        public ActionResult Login()
        {
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult Login(AccountLoginModel model)
        {
            var context = new StackOverflowContext();
            var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
            if (account != null)
            {
                FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);
                return RedirectToAction("Index", "Question");
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Question");
        }

        public ActionResult PasswordRecovery()
        {
            return View(new AccountPasswordRecoveryModel());
        }

        [HttpPost]
        public ActionResult PasswordRecovery(AccountPasswordRecoveryModel model)
        {

            var context = new StackOverflowContext();
            var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email);
            if (account != null)
            {
                return RedirectToAction("AccountPasswordInfo",account);
            }
            return View(model);
           
        }

        public ActionResult AccountPasswordInfo(AccountPasswordInfoModel model, Account account)
        {
            if (account != null)
            {
                model.password = account.Password;
                return View(model);
            }
            return View(new AccountPasswordInfoModel());
        }
      
   
        //public ActionResult NewPassword()
        //{
            
        //    return View(new AccountNewPasswordModel());
        //}
        //[HttpPost]
        //public ActionResult NewPassword(AccountNewPasswordModel model,Account account)
        //{
        //    var context = new StackOverflowContext();
        //    Account newAccount = account;
        //    newAccount.Password = model.Password;
        //    if (account != null)
        //    {
        //        context.Entry(account).CurrentValues.SetValues(newAccount);
        //        context.SaveChanges();
        //        return RedirectToAction("Login");
        //    }
        //    return View(model);
            
        //}


        public ActionResult Profile(ProfileModel model,Guid ID)
        {
            var context = new StackOverflowContext();
         
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == ID).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == ID).Name;
                model.Reputacion = 0;

            return View(model); 
        }

        public ActionResult UserProfile(ProfileModel model)
        {
            var context = new StackOverflowContext();
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid UserId = Guid.Parse(ticket.Name);
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == UserId).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == UserId).Name;
            }
            return View(model);
        }
       

    }

  
}
