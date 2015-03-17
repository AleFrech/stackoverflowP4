using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Data.Migrations;
using StackOverflow.Domain;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;
using WebGrease.Css.Extensions;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace StackOverflow.Web.Controllers{


    public class AccountController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private UnitOfWork unit = new UnitOfWork();

        public AccountController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            
        }
        public ActionResult Register()
        {
            var model = new AccountRegisterModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterModel model )
        {      
            if (ModelState.IsValid)
            {
                var context = new StackOverflowContext();
                var account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                account.Password = EncruptDecrypt.Encrypt(model.Password);
                account.VerifyEmail = false;
                context.Accounts.Add(account);
                context.SaveChanges();
                EmailVerifcations.SendConfirmationMessage(model.Email,account.Id);
                ViewBag.EmailConfirm = "We have sent you an email with instructions to Verify your Account";
            }
            return  View(model);
        }
   
        public ActionResult EmailConfy(Guid Aid)
        {
            var context = new StackOverflowContext();
            context.Accounts.Find(Aid).VerifyEmail = true;
            context.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            ViewBag.Success = TempData["ss"];
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginModel model)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ViewBag.CapisEmpty = "Captcha answer cannot be empty";
                return View(model);
            }
            RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
               ViewBag.CapWrong = "Incorrect captcha answer";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var context = new StackOverflowContext();
                string pass = EncruptDecrypt.Encrypt(model.Password);
                var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == pass);    
                if (account!=null)
                {
                FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);
                return RedirectToAction("Index", "Question");
                }
                ViewBag.Message = "Invalid Email or Password";
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
            if (ModelState.IsValid)
            {
                if (account != null)
                {
                    Random rnd = new Random();
                    int cod = rnd.Next(10000, 99999);
                    string code = cod.ToString();
                    Guid ID = account.Id;
                    EmailVerifcations.SendForgotPasswordMessage(account.Email, code,ID);
                    Session["code"] = code;
                    ViewBag.EmailVerify = "We have sent you an email with instructions to recover your password";
                }
            }
            return View(model);
           
        }

        public ActionResult VerifyCode()
        {
            return View(new VerifyCodeModel());
        }
        [HttpPost]
        public ActionResult VerifyCode(VerifyCodeModel model,Guid ID)
        {
            var Code = Session["code"].ToString();
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
                TempData["ss"]="You Have Succesfully Change your Password";
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            return View(model);
        }

   
        public ActionResult Profile(ProfileModel model,Guid ID)
        {
            var context = new StackOverflowContext();
                CalculateReputation(ID);
                model.Reputacion = context.Accounts.FirstOrDefault(x => x.Id == ID).Reputation;
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == ID).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == ID).Name;
                model.ImageUrl = context.Accounts.FirstOrDefault(x => x.Id == ID).ImageUrl;
                model.LastName = context.Accounts.FirstOrDefault(x => x.Id == ID).LastName;

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
                CalculateReputation(UserId);
                model.Reputacion = context.Accounts.FirstOrDefault(x => x.Id == UserId).Reputation;
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == UserId).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == UserId).Name;
                model.LastName = context.Accounts.FirstOrDefault(x => x.Id == UserId).LastName;
                model.ImageUrl = context.Accounts.FirstOrDefault(x => x.Id == UserId).ImageUrl;
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
                model.LastName = context.Accounts.FirstOrDefault(x => x.Id == UserId).LastName;
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
                context.Accounts.FirstOrDefault(x => x.Id == UserId).LastName = model.LastName;
                context.Accounts.FirstOrDefault(x => x.Id == UserId).ImageUrl = model.ImageUrl;
                context.SaveChanges();
                return RedirectToAction("UserProfile");
            }
            return RedirectToAction("UserProfile");
          
        }

        private void CalculateReputation(Guid OwnerId)
        {
            var context = new StackOverflowContext();
            var answers = context.Answers;
            var questions = context.Questions;
            int Vquestion = 0;
            int Vanswer = 0;
            int Marked = 0;
            
            foreach (var q in questions)
            {
                if (q.Owner.Id == OwnerId)
                    Vquestion += q.Votes;
            }
            foreach (var a in answers)
            {
                if (a.Owner.Id == OwnerId)
                    Vanswer += a.Votes;
                if (a.Marked)
                    Marked+=2;
            }
            context.Accounts.FirstOrDefault(x => x.Id == OwnerId).Reputation = ((Vanswer + Vquestion)/5)+Marked;
            context.SaveChanges();
        }

    }
}
