﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Invoice_web_app.Models
{
    public partial class User
    {
        [Key]
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string pwd { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public byte[]? avatar { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
    public class UserDBContext : DbContext
    {
        public UserDBContext()
        { }
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }

        public DbSet<User> Tuser { get; set; }
    }
}
