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

namespace StackOverflow.Web.Controllers
{
    public class AnswerController : Controller
    {
         private readonly IMappingEngine _mappingEngine;
         public AnswerController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }
        // GET: Answer
        public ActionResult AnswerIndex(AnswerListModel model)
        {
            List<AnswerListModel>models=new List<AnswerListModel>();
            var context = new StackOverflowContext();
            int count = 1;
            foreach (Answer a in context.Answers)
            {

                AnswerListModel answer = new AnswerListModel();
                answer.AnswerCount = "Answer" + count++;
                answer.OwnerID = a.Owner.Id;
                answer.OwnerName = a.Owner.Name;
                answer.CreationDate = a.CreationDate;
                answer.ModificationDate = a.ModififcationnDate;
                answer.Votes = a.Votes;
                answer.AnswerID=new Guid();
                models.Add(answer);

            }
            return View(models);
        }

        public ActionResult CreateAnswer()
        {
            return View(new AnswerCreateModel());
        }

        [HttpPost]
        public ActionResult CreateAnswer(AnswerCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = _mappingEngine.Map<AnswerCreateModel,Answer>(model);
                var context = new StackOverflowContext();
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    Guid ownerId = Guid.Parse(ticket.Name);
                    answer.CreationDate = DateTime.Now;
                    answer.ModififcationnDate = DateTime.Now;
                    answer.Votes = 0;
                    answer.Owner = context.Accounts.FirstOrDefault(x => x.Id == ownerId);
                    context.Answers.Add(answer);
                    context.SaveChanges();
                }

                return RedirectToAction("AnswerIndex");

            }
            return View(model);
          
        }

        public ActionResult AnswerDetails(Guid ID)
        {
            var context = new AnswerDetailModel();
        }
    }
}