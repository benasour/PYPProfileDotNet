using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PYPProfileDotNet.Models
{
    public class Friend
    {
        public int user_id1 { get; set; };
        public int user_id2 { get; set; };
        public int status_id { get; set; }; 
    }
    public class FriendDBContext : DbContext
    {
        public DbSet<Friend> Friend { get; set; }
    }

}
