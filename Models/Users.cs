using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBankAccounts.Models
{
    public class User : BasicEntity
    {
        public int Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public int AccountId {get; set;}
        public Account Account {get; set;}
        public User() : base()
        {}
        public User(RegistrationViewModel data) : base()
        {
            this.FirstName = data.FirstName;
            this.LastName = data.LastName;
            this.Email = data.Email;
            this.Password = data.Password;
        }
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email {get; set;}
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(45, ErrorMessage = "First Name must not be more than 45 characters")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(45, ErrorMessage = "Last Name must not be more than 45 characters")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email {get; set;}
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password {get; set;}
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Password must match confirmation")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get; set;}
    }
}