using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PYPProfileDotNet.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
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
        [Remote("IsUniqueUserName", "Account")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password must be between 6 and 100 characters")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password are not the same")]
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

    public class UserAccount
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "User Name")]
        [Remote("IsUniqueUserNameExcludingCurrentUserName", "Account")]
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }

    public class ChangePassword
    {
        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password must be between 6 and 100 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "New Password Confirmation")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The password and confirmation password are not the same")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
    #endregion

}