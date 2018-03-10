using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SeniorProject.Models
{
    public class User
    {
        public string userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string profilepic { get; set; }
        public string role { get; set; }
        public DateTime registerdate { get; set; }
    }
}