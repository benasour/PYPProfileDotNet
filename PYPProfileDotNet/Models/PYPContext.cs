using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PYPProfileDotNet.Models
{
    public class PYPContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<FriendStatus> FriendStatus { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<History> History { get; set; }
    }
}