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
        public int tempID { get; set; }
       /* [Key, Column(Order = 0)]
        public User user1 { get; set; }
        [Key, Column(Order = 1)]
        public User user2 { get; set; }
        * */
        public int user_id1 { get; set; }
        public int user_id2 { get; set; }
        public int status_id { get; set; }
    }

    public class FriendResult
    {
        public int tempID { get; set; }
        public int friendStatus { get; set; }
        public string friendName { get; set; }
    }
}
