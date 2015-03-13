﻿using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class ProfileModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid UserID { get; set; }

        public string ImageUrl { get; set; }
        public int Reputacion { get; set; }

    }
}