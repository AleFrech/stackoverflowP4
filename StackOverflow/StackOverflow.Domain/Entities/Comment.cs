using System;

namespace StackOverflow.Domain.Entities
{
    public class Comment : IEntity
    {
        public Guid Id { get; private set; }
        public virtual Account Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public Guid FatherId { get; set; }
        public int Votes { get; set; }
        public Comment()
        {
            Id = Guid.NewGuid();
        }

    }
}