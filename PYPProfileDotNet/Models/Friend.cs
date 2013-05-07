using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PYPProfileDotNet.Models
{
    public class Friend
    {
        [Key]
        public int id { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

        [Display(Name = "Friend Status")]
        public virtual FriendStatus Status { get; set; }
    }

    public class FriendResult
    {
        public int id { get; set; }
        public string friendStatus { get; set; }
        public int friendStatusId { get; set; }

        [Display(Name = "User Name")]
        public string userName { get; set; }

        [Display(Name = "Friend Name")]
        public string friendName { get; set; }
    }
}
