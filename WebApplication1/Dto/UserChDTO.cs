﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entitites
{
    public class UserChDTO
    {
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public string? social_login_provider { get; set; }
    }
}
