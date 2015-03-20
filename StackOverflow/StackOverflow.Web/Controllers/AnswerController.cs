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
                    answer.ImageUrl = context.Accounts.Find(a.Owner.Id).ImageUrl;
                    models.Add(answer); 
                }
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    Guid ownerId = Guid.Parse(ticket.Name);
                    ViewData["loginUser"] = ownerId;
                }

            }
            models = models.OrderByDescending(x => x.Votes).ThenByDescending(x => x.CreationDate).ToList();
            if (models.Find(x => x.Marked == true) != null)
            {    
                var tmp = models.Find(x => x.Marked == true);
                models.Remove(models.Find(x => x.Marked == true));
                models.Insert(0, tmp);
                tmp = null;     
            }        
            return PartialView(models);
        }
        
        public ActionResult CreateAnswer()
        {
            return PartialView(new AnswerCreateModel());
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateAnswer(QuestionDetailModel model,string qID)
        {
            if (model.CreateAnswer != null)
            {
                var answer = new Answer();
                var context = new StackOverflowContext();
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    Guid ownerId = Guid.Parse(ticket.Name);
                    answer.CreationDate = DateTime.Now;
                    answer.ModififcationnDate = DateTime.Now;
                    answer.Description = model.CreateAnswer;
                    answer.Votes = 0;
                    answer.Owner = context.Accounts.FirstOrDefault(x => x.Id == ownerId);
                    answer.QuestionId = Guid.Parse(qID);
                    context.Answers.Add(answer);
                    context.SaveChanges();
                }

            }
            return RedirectToAction("QuestionDetail", "Question", new { ID = Guid.Parse(qID) });
          
        }

        public ActionResult UploadAnswer( Guid qID,string desc)
        {
            
            var answer=new Answer();
            var context = new StackOverflowContext();
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                answer.CreationDate = DateTime.Now;
                answer.ModififcationnDate = DateTime.Now;
                answer.Description = desc;
                answer.Votes = 0;
                answer.Owner = context.Accounts.FirstOrDefault(x => x.Id == ownerId);
                answer.QuestionId = qID;
                context.Answers.Add(answer);
                context.SaveChanges();
            }
            return RedirectToAction("QuestionDetail", "Question", new { ID = qID });

        }

        public ActionResult AnswerDetails(Guid ID,string qID)
        {
            var context = new StackOverflowContext();
            var answer = context.Answers.Find(ID);
            AnswerDetailModel model = _mappingEngine.Map<Answer, AnswerDetailModel>(answer);
            model.AnswerID = ID;
            model.QuestionID = Guid.Parse(qID);
            return PartialView(model);

        }

        [System.Web.Mvc.Authorize]
        public ActionResult UpVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Answers.Find(ID).Votes++;
            context.SaveChanges();
            return RedirectToAction("QuestionDetail", "Question", new {ID = context.Answers.Find(ID).QuestionId});
        }

        [System.Web.Mvc.Authorize]
        public ActionResult DownVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Answers.Find(ID).Votes--;
            context.SaveChanges();
            return RedirectToAction("QuestionDetail","Question", new {ID = context.Answers.Find(ID).QuestionId });
        }

        [System.Web.Mvc.Authorize]
        public ActionResult MarkAnswer(Guid ID,Guid qId)
        {
            var context = new StackOverflowContext();
            var questions = context.Questions.Find(qId);
            var answer = context.Answers.Find(ID);
            var answers = context.Answers;
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                foreach (Answer a in answers)
                {
                    if (a.QuestionId == qId)
                    {
                        a.Marked = false;
                    }
                }
                questions.HavedMark = false;
                if (!questions.HavedMark && questions.Owner.Id == ownerId)
                {
                    answer.Marked = true;
                    questions.HavedMark = true;
                    context.SaveChanges();
                }
               
            }
            return RedirectToAction("QuestionDetail", "Question", new { ID = qId });
        }

        [System.Web.Mvc.Authorize]
        public ActionResult DelteAnswer(Guid ID)
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
                }    
            }

            return RedirectToAction("QuestionDetail", "Question", new { ID = answer.QuestionId });

        }
    }
}