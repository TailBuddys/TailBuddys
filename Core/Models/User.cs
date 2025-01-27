using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Numerics;
using System.Xml;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TailBuddys.Core.Models
{
    public class User
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Dog> Dogs { get; set; } = new List<Dog>();
        public string GoogleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

}
