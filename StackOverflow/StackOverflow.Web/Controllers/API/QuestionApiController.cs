using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers.API
{
    public class QuestionApiController : ApiController
    {
        public IEnumerable<QuestionListModel> Get()
        {
            List<QuestionListModel> models = new List<QuestionListModel>();
            var context = new StackOverflowContext();
            foreach (var q in context.Questions)
            {
                QuestionListModel question = new QuestionListModel();
                question.Title = q.Title;
                question.OwnerID = q.Owner.Id;
                question.OwnerName = q.Owner.Name + " " + q.Owner.LastName;
                question.CreationDate = q.CreationDate;
                question.Votes = q.Votes;
                question.Views = q.Views;
                question.Answers = q.Answers;
                question.QuestionID = q.Id;
                question.ImageUrl = context.Accounts.Find(q.Owner.Id).ImageUrl;
                models.Add(question);
            }
            return models;
        }
        public HttpResponseMessage PostNewQuestion(NewQuestionModel model)
        {
            if (!model.Title.IsNullOrWhiteSpace() && !model.Description.IsNullOrWhiteSpace()&&System.Web.HttpContext.Current.Session["ApiUser"]!=null)
            {
                    var question = Mapper.Map<NewQuestionModel, Question>(model);
                    var context = new StackOverflowContext();
                    Guid ownerid = Guid.Parse(System.Web.HttpContext.Current.Session["ApiUser"].ToString());
                    question.CreationDate = DateTime.Now;
                    question.ModififcationnDate = DateTime.Now;
                    question.Votes = 0;
                    question.Views = 0;
                    question.Answers = 0;
                    question.Owner = context.Accounts.FirstOrDefault(x => x.Id == ownerid);
                    context.Questions.Add(question);
                    context.SaveChanges();
                    HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;

            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
        }

        public QuestionDetailModel Get(Guid ID)
        {
            var context = new StackOverflowContext();
            var question = context.Questions.FirstAsync(x => x.Id == ID);
            var  model = new QuestionDetailModel();
            model.Title = context.Questions.FirstOrDefault(x => x.Id == ID).Title;
            model.Decription =context.Questions.FirstOrDefault(x => x.Id == ID).Description;
            model.QuestionId = ID;
            model.Owner = context.Questions.FirstOrDefault(x => x.Id == ID).Owner;
            model.Votes = context.Questions.FirstOrDefault(x => x.Id == ID).Votes;
            model.Views = context.Questions.FirstOrDefault(x => x.Id == ID).Views;     
            int cont = Enumerable.Count(context.Answers, a => a.QuestionId == ID);
            context.Questions.Find(ID).Answers=cont;
            model.Answers = context.Questions.FirstOrDefault(x => x.Id == ID).Answers;
            context.SaveChanges();

            return model;

        }
    }
}