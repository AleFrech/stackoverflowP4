using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class AnswerDetailModel
    {
        public string Description { set; get; }
        public int Votes { set; get; }
        public Guid AnswerID { get; set; }
    }
}