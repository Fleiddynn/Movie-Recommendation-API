﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entitites
{
    public class UserDTO : IdentityUser<Guid>
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public UserDTO(User u)
        {
            email = u.email;
            first_name = u.first_name;
            last_name = u.last_name;
        }
    }
}
