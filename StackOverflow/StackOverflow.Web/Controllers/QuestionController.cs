using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Controllers;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class QuestionController : Controller
    {
         private readonly IMappingEngine _mappingEngine;
        public QuestionController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }
        
        public ActionResult Index()
        {
            List<QuestionListModel>models =new List<QuestionListModel>();
            var context = new StackOverflowContext();
            foreach (var q in context.Questions)
            {

                QuestionListModel question = new QuestionListModel();
                question.Title = q.Title;
                question.OwnerID = q.Owner.Id;
                question.OwnerName = q.Owner.Name;
                question.CreationDate = q.CreationDate;
                question.Votes = q.Votes;
                question.Views = q.Views;
                question.Answers = q.Answers;
                question.QuestionID = q.Id;
                question.ImageUrl = context.Accounts.Find(q.Owner.Id).ImageUrl;
                models.Add(question);

            }

            models=models.OrderByDescending(x => x.CreationDate).ToList();
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName]; 
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                ViewData["loginUser"] = ownerId; 
            }

 
            return View(models);
        }
        [Authorize]
        public ActionResult NewQuestion()
        {
            return View(new NewQuestionModel());
        }

        [HttpPost]
        public ActionResult NewQuestion(NewQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                var question = _mappingEngine.Map<NewQuestionModel,Question>(model);
                var context = new StackOverflowContext();
                 HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if(cookie!=null)
                {
                     FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                     Guid ownerId = Guid.Parse(ticket.Name);
                     question.CreationDate = DateTime.Now;
                     question.ModififcationnDate = DateTime.Now;
                     question.Votes = 0;
                     question.Views = 0;
                     question.Answers = 0;
                     question.Owner = context.Accounts.FirstOrDefault(x=>x.Id==ownerId);
                     context.Questions.Add(question);        
                     context.SaveChanges();
                }
              
               return RedirectToAction("Index");            
                
            }
            return View(model);

       
        }
        [AllowAnonymous]
        public ActionResult QuestionDetail( Guid ID)
        {
            ViewData["id"] = ID.ToString();
            TempData["qID"] = ID;
            if (TempData["AnswerBelow5word"] != null)
                ViewData["AnswerBelow5word"] = TempData["AnswerBelow5word"];
            
            addQuestionViews(ID);
            QuestionDetailModel model = new QuestionDetailModel();
            if (ModelState.IsValid)
            {
                var context = new StackOverflowContext();
                model.Title = context.Questions.FirstOrDefault(x => x.Id == ID).Title;
                model.Decription = context.Questions.FirstOrDefault(x => x.Id == ID).Description;
                model.QuestionId = ID;
                model.Votes = context.Questions.FirstOrDefault(x => x.Id == ID).Votes;
                model.Views = context.Questions.FirstOrDefault(x => x.Id == ID).Views;
                int cont = Enumerable.Count(context.Answers, a => a.QuestionId == ID);
                context.Questions.Find(ID).Answers=cont;
                context.SaveChanges();
                model.Answers = context.Questions.FirstOrDefault(x => x.Id == ID).Answers;
                return View(model);

            }
            
            return View(model);
        }

        [Authorize]
        public ActionResult UpVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Questions.Find(ID).Votes++;
            context.SaveChanges();
            return RedirectToAction("QuestionDetail", new { ID = ID });
        }
        [Authorize]
        public ActionResult DownVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Questions.Find(ID).Votes--;
            context.SaveChanges();
            return RedirectToAction("QuestionDetail", new { ID = ID });
        }
        [Authorize]
        public ActionResult DeleteQuestion(Guid ID)
        {
            var context = new StackOverflowContext();
            var question = context.Questions.Find(ID);
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                if (question.Owner.Id == ownerId)
                {
                    foreach (Answer a in context.Answers)
                    {
                        if (a.QuestionId == ID)
                        {
                            context.Answers.Remove(a);
                        } 
                    }
                    foreach (Comment c in context.Comments)
                    {
                        if (c.FatherId == ID)
                        {
                            context.Comments.Remove(c);
                        }
                    }
                    context.Questions.Remove(question);

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                

            }

            return RedirectToAction("QuestionDetail", new { ID = ID });

        }

        private void addQuestionViews(Guid qId)
        {
            var context = new StackOverflowContext();
            context.Questions.Find(qId).Views++;
            context.SaveChanges();         
        }
       
    }
}