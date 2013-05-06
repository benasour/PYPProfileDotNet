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
        public virtual FriendStatus Status { get; set; }
    }

    public class FriendResult
    {
        public int id { get; set; }
        public string friendStatus { get; set; }
        public int friendStatusId { get; set; }
        public string friendName { get; set; }
    }
}
