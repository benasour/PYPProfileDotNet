using System;
using System.ComponentModel.DataAnnotations;
namespace PYPProfileDotNet.Models
{
    public class User
    {
        
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }

    #region UserViewModels
    public class UserRegistration
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password are not the same")]
        [Display(Name = "Confirmation Password")]
        public string PasswordConfirmation { get; set; }
    }

    public class UserLogin
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    #endregion

}