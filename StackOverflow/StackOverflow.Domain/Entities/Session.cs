using System;

namespace StackOverflow.Domain.Entities
{
    public class Session:IEntity
    {
        public Guid Id { get; private set; }

        public Guid LoggedId { get; set; }

        public string token { get; set; }

        public Session()
        {
            Id = Guid.NewGuid();
        }
    }
}