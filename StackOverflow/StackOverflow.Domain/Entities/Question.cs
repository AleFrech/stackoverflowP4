using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
    public class Question :IEntity
    {
        public Guid Id { get; private set;}
        public int Votes { get; set;}
        public string Description { get; set;}
        public virtual Account Owner { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModififcationnDate { get; set; }

        public bool HavedMark { get; set; }


    

        public Question()
        {
            Id = Guid.NewGuid();
        }

    }
}
