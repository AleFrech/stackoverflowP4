using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
    public class Answer:IEntity
    {
        public Guid Id { get; set; }
        public int Votes { get; set; }
        public virtual Account Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModififcationnDate { get; set; }
        public Guid  QuestionId { get; set; }

        public bool Marked = false;

        public string Description{get; set; }

        public Answer()
        {
            Id = Guid.NewGuid();
        }
    }
}
