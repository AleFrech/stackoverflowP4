using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflow.Web.Controllers
{
    public class AnswerController : Controller
    {
        // GET: Answer
        public ActionResult AnswerIndex(AnswerListModel model)
        {
            List<AnswerListModel>models=new List<AnswerListModel>();
            return View(models);
        }
    }
}