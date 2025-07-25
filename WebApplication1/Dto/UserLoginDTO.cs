﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entitites
{
    public class UserLoginDTO : IdentityUser<Guid>
    {
        public required string email { get; set; }
        public required string password { get; set; }
    }
}
