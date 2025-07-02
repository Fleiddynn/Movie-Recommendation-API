using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WebApplication1.Entitites
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string first_name { get; set; } = string.Empty;
        public required string last_name { get; set; } = string.Empty;
        public required string email { get; set; } = string.Empty;
        public required string password { get; set; } = string.Empty;
        public string social_login_provider { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;
    }
}
