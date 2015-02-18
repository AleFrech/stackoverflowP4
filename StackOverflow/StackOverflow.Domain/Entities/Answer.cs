﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
    public class Answer:IEntity
    {
        public Guid Id { get; private set; }
        public int Votes { get; set; }
        public string Owner { get; set; }
        public string Date { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModififcationnDate { get; set; }

        public string Answertxt{get; set; }

        public Answer()
        {
            Id = Guid.NewGuid();
        }
    }
}