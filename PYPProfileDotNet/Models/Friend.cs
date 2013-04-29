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
        public int user_id1 { get; set; }
        public int user_id2 { get; set; }
        public int status_id { get; set; }
    }

    public class FriendResult
    {
        public int id { get; set; }
        public int friendStatus { get; set; }
        public string friendName { get; set; }
    }
}
