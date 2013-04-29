using System.Collections.Generic;

namespace PYPProfileDotNet.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
    }

    #region UserViewModels
    public class UserRegistration
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
    #endregion

}