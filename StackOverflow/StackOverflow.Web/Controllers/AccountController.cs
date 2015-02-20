using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
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
            FormsAuthentication.SetAuthCookie(model.Email,false);
            return RedirectToAction("Index","Question");
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
            return RedirectToAction("PasswordCode");
        }
        public ActionResult PasswordCode()
        {
            return View(new AccountPasswordCodeModel());
        }
        [HttpPost]
        public ActionResult PasswordCode(AccountPasswordCodeModel model)
        {
            
            if(model.Code=="1234")
            return RedirectToAction("NewPassword");

            ViewBag.Me = "Error";
            return View(new AccountPasswordCodeModel());
            
        }
        public ActionResult NewPassword()
        {
            return View(new AccountNewPasswordModel());
        }
        [HttpPost]
        public ActionResult NewPassword(AccountNewPasswordModel model)
        {

            return RedirectToAction("Login");
        }


        public ActionResult Profile()
        {
            ProfileModel modelTest = new ProfileModel();
            modelTest.Name = "Pedro";
            modelTest.Email = "Pedro@example.com";
            modelTest.Reputacion = 100;
            return View(modelTest);
        }
        [HttpPost]
        public ActionResult Profile(ProfileModel model)
        {
            return View(model);
        }
     
    }
}
