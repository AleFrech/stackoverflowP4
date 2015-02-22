using System;

namespace StackOverflow.Web.Controllers
{
    public class AnswerListModel
    {
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public string OwnerName { get; set; }
        public Guid OwnerID { get; set; }
        public Guid QuestionID { get; set; }
        public Guid AnswerID { get; set; }
    }
}