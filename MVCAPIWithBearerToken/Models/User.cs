﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAPIWithBearerToken.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}