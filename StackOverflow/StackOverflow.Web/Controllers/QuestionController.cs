using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<QuestionListModel>models =new List<QuestionListModel>();
            QuestionListModel modelTest = new QuestionListModel();
            modelTest.Title = "title test";
            modelTest.OwnerName="Pedro";
            modelTest.Votes = 1;
            modelTest.CreationDate =DateTime.Now;
            modelTest.OwnerID = Guid.NewGuid();
            modelTest.QuestionID = Guid.NewGuid();

            models.Add(modelTest);
            return View(models);
        }

        public ActionResult NewQuestion()
        {
            return View(new NewQuestionModel());
        }

        [HttpPost]
        public ActionResult NewQuestion(NewQuestionModel model)
        {
            return View( model);
        }

        public ActionResult QuestionDetail()
        {
            return View(new QuestionDetailModel());
        }

        [HttpPost]
        public ActionResult QuestionDetail(QuestionDetailModel model, string Command)
        {
            if (Command == "UpVote")
            {
                model.Votes = model.Votes + 1;
 
            }

            return View(model);
        }
       
    }
}