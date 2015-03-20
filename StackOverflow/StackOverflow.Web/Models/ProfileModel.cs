using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.Models
{
    public class ProfileModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RegistrationDate { get; set; }
        public int Views { get; set; }
        public string LastTimeSeen { get; set; }
        public Guid UserID { get; set; }
        public string ImageUrl { get; set; }
        public int Reputacion { get; set; }
        public List<Question> QuestionList { get; set; }
        public List<Answer> AnswerList { get; set; } 

    }
}