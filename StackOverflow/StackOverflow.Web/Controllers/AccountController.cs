using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain;
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
            var model = new AccountRegisterModel();
            model.Error = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterModel model )
        {
             var  context = new StackOverflowContext();
            foreach (Account a in context.Accounts)
            {
                if (model.Email == a.Email)
                {
                    model.Error="same email";
                    return View(model);
                }                    
            }
            if (ModelState.IsValid)
            {
                var account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                account.Password = EncruptDecrypt.Encrypt(model.Password);
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
            string pass= EncruptDecrypt.Encrypt(model.Password);
            var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == pass);
            if (account != null)
            {
                FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);
                return RedirectToAction("Index", "Question");
            }
            return View(new AccountLoginModel());
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
                Random rnd = new Random();
                int cod = rnd.Next(10000, 99999);
                string code = cod.ToString();
                Guid ID = account.Id;
                EmailVerifcations.SendSimpleMessage(account.Email, code);
                return RedirectToAction("VerifyCode", new { ID = ID ,Code=code});
            }
            return View(model);
           
        }

        public ActionResult VerifyCode()
        {
            return View(new VerifyCodeModel());
        }
        [HttpPost]
        public ActionResult VerifyCode(VerifyCodeModel model,string Code,Guid ID)
        {
           
            if (Code.Equals(model.code))
            {
                return RedirectToAction("ChangePassword", new { ID = ID });
            }
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View(new AccountChangePasswordModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(AccountChangePasswordModel model,Guid ID)
        {
            if (ModelState.IsValid)
            {
                var context = new StackOverflowContext();
                context.Accounts.Find(ID).Password = EncruptDecrypt.Encrypt(model.Password);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

   
        public ActionResult Profile(ProfileModel model,Guid ID)
        {
            var context = new StackOverflowContext();
         
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == ID).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == ID).Name;
                model.Reputacion = 0;

            return View(model); 
        }
        [Authorize]
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
                model.UserID = UserId;
 
 
            }
            return View(model);
        }

        public ActionResult EditProfile()
        {
            var model = new EditProfileModel();

            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
             var context = new StackOverflowContext();
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid UserId = Guid.Parse(ticket.Name);
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == UserId).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == UserId).Name;
                return View(model);
            }
            return RedirectToAction("UserProfile");
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileModel model)
        {

            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var context = new StackOverflowContext();
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid UserId = Guid.Parse(ticket.Name);
                context.Accounts.FirstOrDefault(x => x.Id == UserId).Email=model.Email;
                context.Accounts.FirstOrDefault(x => x.Id == UserId).Name=model.Name;
                context.SaveChanges();
                return RedirectToAction("UserProfile");
            }
            return RedirectToAction("UserProfile");
          
        }

    }

  
}
