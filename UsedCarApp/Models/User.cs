using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsedCarApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }

    }
}