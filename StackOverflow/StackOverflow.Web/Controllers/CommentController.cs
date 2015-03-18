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
    public class CommentController : Controller
    {
        private readonly IMappingEngine _mappingEngine;

        public CommentController(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public ActionResult CommentIndex(Guid Fid)
        {
            List<CommentListModel> models = new List<CommentListModel>();
            var context = new StackOverflowContext();
            foreach (Comment c in context.Comments)
            {
                CommentListModel comment = new CommentListModel();
                if (c.FatherId == Fid)
                {
                    comment.CreationDate = c.CreationDate;
                    comment.Description = c.Description;
                    comment.OwnerId = c.Owner.Id;
                    comment.OwnerName = c.Owner.Name;
                    comment.CommentId = c.Id;
                    comment.Votes = c.Votes;
                    models.Add(comment);
                }
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    Guid ownerId = Guid.Parse(ticket.Name);
                    ViewData["loginUser"] = ownerId;
                }
               
            }
            return PartialView(models);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult CreateComment()
        {
            return View(new CommentCreateModel());
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateComment(CommentCreateModel model, string Fid)
        {
            ViewData["Fid"] = Fid;
            if (ModelState.IsValid)
            {
                var comment = _mappingEngine.Map<CommentCreateModel, Comment>(model);
                var context = new StackOverflowContext();
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    Guid ownerId = Guid.Parse(ticket.Name);
                    comment.CreationDate = DateTime.Now;
                    comment.Votes = 0;
                    comment.Owner = context.Accounts.FirstOrDefault(x => x.Id == ownerId);
                    comment.FatherId = Guid.Parse(Fid);
                    context.Comments.Add(comment);
                    context.SaveChanges();
                }

            }
            return RedirectToAction("QuestionDetail", "Question", new { ID = Guid.Parse(Fid)});

        }
        [System.Web.Mvc.Authorize]
        public ActionResult UpVote(Guid ID)
        {
            var context = new StackOverflowContext();
            context.Comments.Find(ID).Votes++;
            context.SaveChanges();
            return RedirectToAction("QuestionDetail", "Question", new { ID = context.Comments.Find(ID).FatherId });
        }
        [System.Web.Mvc.Authorize]
        public ActionResult DelteComment(Guid ID)
        {
            var context = new StackOverflowContext();
            var comment = context.Comments.Find(ID);
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Guid ownerId = Guid.Parse(ticket.Name);
                if (comment.Owner.Id == ownerId)
                {
                    context.Comments.Remove(comment);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("QuestionDetail", "Question", new { ID = comment.FatherId });

        }
    }
}