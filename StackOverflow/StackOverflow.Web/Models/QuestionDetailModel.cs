using System;
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Controllers
{
    public class QuestionDetailModel
    {
        public string Title { get; set; }
        public string Decription { get; set; }

        public int Votes { get; set; }
        public int Views { get; set; }
        [Required]
        public string CeateAnswer { get; set; }
       [Required]
        public string CreateComment { get;set; }
        public Guid QuestionId { set; get; }

    }
}