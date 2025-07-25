﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entitites
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Review> UserReviews { get; set; } = new List<Review>();
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public string? social_login_provider { get; set; }
        public List<Guid>? watchedMovies { get; set; } = new List<Guid>();
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
