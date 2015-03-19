using System;

namespace StackOverflow.Web.Controllers
{
    public class QuestionDetailModel
    {
        public string Title { get; set; }
        public string Decription { get; set; }

        public int Votes { get; set; }
        public int Views { get; set; }

        public Guid QuestionId { set; get; }

    }
}