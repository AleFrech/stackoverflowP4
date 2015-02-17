using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class QuestionListModel
    {
        public string Title { get; set; }
        public string Votes { get; set; }
        public string CreationDate { get; set; }
        public string OwnerName { get; set; }
    }
}