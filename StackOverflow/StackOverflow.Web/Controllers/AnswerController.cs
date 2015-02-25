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
        public ActionResult AnswerIndex(Guid qID)
        {
            ViewData["id"] = qID.ToString();
            List<AnswerListModel>models=new List<AnswerListModel>();
            var context = new StackOverflowContext();
            int count = 1;
            foreach (Answer a in context.Answers)
            {
                var question = context.Questions.Find(qID);
                AnswerListModel answer = new AnswerListModel();
                if (a.QuestionId == qID)
                {
                    answer.AnswerCount = "Answer " + count++;
                    answer.OwnerID = a.Owner.Id;
                    answer.OwnerName = a.Owner.Name;
                    answer.CreationDate = a.CreationDate;
                    answer.ModificationDate = a.ModififcationnDate;
                    answer.Votes = a.Votes;
                    answer.AnswerID = a.Id;
                    answer.Marked = a.Marked;
                    models.Add(answer); 
                }

            }
            return View(models);
        }
        [Authorize]
        public ActionResult CreateAnswer()
        {
            return View(new AnswerCreateModel());
        }

        [HttpPost]
        public ActionResult CreateAnswer(AnswerCreateModel model, string qID)
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
                    answer.QuestionId = Guid.Parse(qID);
                    context.Answers.Add(answer);
                    context.SaveChanges();
                }

                return RedirectToAction("AnswerIndex",new{qID=qID});

            }
            return View(model);
          
        }

        public ActionResult AnswerDetails(Guid ID,string qID)
        {
            var context = new StackOverflowContext();
            var answer = context.Answers.Find(ID);
            AnswerDetailModel model = _mappingEngine.Map<Answer, AnswerDetailModel>(answer);
            model.AnswerID = ID;
            model.QuestionID = Guid.Parse(qID);
            return View(model);

        }

        [Authorize]
        public ActionResult UpVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Answers.Find(ID).Votes++;
            context.SaveChanges();
           return  RedirectToAction("AnswerDetails", new{ID=ID,qID=context.Answers.Find(ID).QuestionId});
        }

        [Authorize]
        public ActionResult DownVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Answers.Find(ID).Votes--;
            context.SaveChanges();
            return RedirectToAction("AnswerDetails", new { ID = ID, qID = context.Answers.Find(ID).QuestionId });
        }

        [Authorize]
        public ActionResult MarkAnswer(Guid ID,Guid qId)
        {
            var context = new StackOverflowContext();
            var questions = context.Questions.Find(qId);
            var answer = context.Answers.Find(ID);
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                if (!questions.HavedMark && questions.Owner.Id ==ownerId)
                {
                    answer.Marked = true;
                    questions.HavedMark = true;

                    context.SaveChanges();
                }
            }

            return RedirectToAction("AnswerIndex",new{qID=qId});
        }

        [Authorize]
        public ActionResult DelteAnswer(Guid ID,Guid qID)
        {
            var context = new StackOverflowContext();
            var answer = context.Answers.Find(ID);
              HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                if (answer.Owner.Id == ownerId)
                {
                    context.Answers.Remove(answer);
                    context.SaveChanges();
                    return RedirectToAction("AnswerIndex", new {qID = answer.QuestionId});
                }
                   return RedirectToAction("AnswerDetails",new{ID=ID,qID=qID.ToString()});

            }

            return RedirectToAction("AnswerDetails",new{ID=ID,qID=qID.ToString()});


        }
    }
}