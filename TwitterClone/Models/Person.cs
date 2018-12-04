using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public System.DateTime Joined { get; set; }
        public bool Active { get; set; }
    }
}