using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;
using WebGrease.Css.Extensions;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using StackOverflow.Web.CostumeDataNotations;

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
                account.CreationDate =(DateTime.Now).ToString();
                account.ProfileViews = 0;
                account.LasTimeSeen = DateTime.Now.ToString();
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
            context.Accounts.FirstOrDefault(x=>x.Id==Aid).VerifyEmail = true;
            context.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            Session["att"] = 0;
            ViewBag.Success = TempData["ss"];
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginModel model)
        {

            
            ViewData["cont"] = 0;
            if (ModelState.IsValid)
            {

                if(Session!= null){
                    if (Session["att"] != null)
                    {
                        if (((int) Session["att"]) >= 3)
                        {
                            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
                            if (String.IsNullOrEmpty(recaptchaHelper.Response))
                            {
                                ViewBag.CapisEmpty = "Captcha answer cannot be empty";
                                return View(model);
                            }
                            RecaptchaVerificationResult recaptchaResult =
                                await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
                            if (recaptchaResult != RecaptchaVerificationResult.Success)
                            {
                                ViewBag.CapWrong = "Incorrect captcha answer";
                                return View(model);
                            }
                        }
                    }
                }

            var context = new StackOverflowContext();
                string pass = EncruptDecrypt.Encrypt(model.Password);
                var account = context.Accounts.FirstOrDefault(x => x.Email == model.Email && x.Password == pass);    
                if (account!=null)
                {
                FormsAuthentication.SetAuthCookie(account.Id.ToString(), false);
                account.LasTimeSeen = DateTime.Now.ToString();
                context.SaveChanges();
                
                return RedirectToAction("Index", "Question");
                }
                ViewBag.Message = "Invalid Email or Password";
                if (Session != null)
                {
                    if (Session["att"] != null)
                    {
                        int a = int.Parse(Session["att"].ToString());
                        a++;
                        Session["att"] = a;
                    }
                }
                else
                {
                    Session["att"] = 0;
                }
                

               foreach (var a in context.Accounts.Where(a => a.Email.Equals(model.Email)))
               {
                    EmailVerifcations.SendAlertMessage(model.Email);
               }

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
                context.Accounts.Find(ID).ProfileViews++;
                context.SaveChanges();
                model.Reputacion = context.Accounts.FirstOrDefault(x => x.Id == ID).Reputation;
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == ID).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == ID).Name;
                model.ImageUrl = context.Accounts.FirstOrDefault(x => x.Id == ID).ImageUrl;
                model.LastName = context.Accounts.FirstOrDefault(x => x.Id == ID).LastName;
                model.RegistrationDate = TimetoRelative(DateTime.Parse(context.Accounts.FirstOrDefault(x => x.Id == ID).CreationDate));
                model.LastTimeSeen = TimetoRelative(DateTime.Parse(context.Accounts.FirstOrDefault(x => x.Id == ID).LasTimeSeen));
                model.Views = context.Accounts.FirstOrDefault(x => x.Id == ID).ProfileViews;
            List<Question> tmpQ = context.Questions.Where(q => q.Owner.Id == ID).ToList();
            tmpQ = tmpQ.OrderByDescending(x => x.CreationDate).ToList();
            List<Answer> tmpA = context.Answers.Where(a => a.Owner.Id == ID).ToList();
            tmpA = tmpA.OrderByDescending(x => x.CreationDate).ToList();
            if (tmpQ.Count != 0)
                model.QuestionList = tmpQ.Take(5).ToList();
            if (tmpA.Count != 0)
                model.AnswerList = tmpA.Take(5).ToList();
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
                context.Accounts.Find(UserId).ProfileViews++;
                context.SaveChanges();
                model.Reputacion = context.Accounts.FirstOrDefault(x => x.Id == UserId).Reputation;
                model.Email = context.Accounts.FirstOrDefault(x => x.Id == UserId).Email;
                model.Name = context.Accounts.FirstOrDefault(x => x.Id == UserId).Name;
                model.LastName = context.Accounts.FirstOrDefault(x => x.Id == UserId).LastName;
                model.ImageUrl = context.Accounts.FirstOrDefault(x => x.Id == UserId).ImageUrl;
                model.RegistrationDate = TimetoRelative(DateTime.Parse(context.Accounts.FirstOrDefault(x => x.Id ==UserId).CreationDate));
                model.LastTimeSeen = TimetoRelative(DateTime.Parse(context.Accounts.FirstOrDefault(x => x.Id == UserId).LasTimeSeen));
                model.Views = context.Accounts.FirstOrDefault(x => x.Id == UserId).ProfileViews;
                model.UserID = UserId;
                List<Question> tmpQ = context.Questions.Where(q => q.Owner.Id == UserId).ToList();
                tmpQ = tmpQ.OrderByDescending(x => x.CreationDate).ToList();
                List<Answer> tmpA = context.Answers.Where(a => a.Owner.Id == UserId).ToList();
                tmpA = tmpA.OrderByDescending(x => x.CreationDate).ToList();
                if (tmpQ.Count != 0)
                    model.QuestionList = tmpQ.Take(5).ToList();
                if (tmpA.Count != 0)
                    model.AnswerList = tmpA.Take(5).ToList();
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

        private string TimetoRelative(DateTime dt)
        {
            TimeSpan timeSince = DateTime.Now.Subtract(dt);
            if (timeSince.TotalMilliseconds < 1) return "not yet";
            if (timeSince.TotalMinutes < 1) return "just now";
            if (timeSince.TotalMinutes < 2) return "1 minute ago";
            if (timeSince.TotalMinutes < 60) return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return "1 hour ago";
            if (timeSince.TotalHours < 24) return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays < 2) return "yesterday";
            if (timeSince.TotalDays < 7) return string.Format("{0} days ago", timeSince.Days);
            if (timeSince.TotalDays < 14) return "last week";
            if (timeSince.TotalDays < 21) return "2 weeks ago";
            if (timeSince.TotalDays < 28) return "3 weeks ago";
            if (timeSince.TotalDays < 60) return "last month";
            if (timeSince.TotalDays < 365) return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return "last year"; //last but not least...
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));    
        }

    }
}


