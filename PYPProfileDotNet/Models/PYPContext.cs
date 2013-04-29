using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PYPProfileDotNet.Models
{
    public class PYPContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FriendStatus> FriendStatuses { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<History> History { get; set; }
    }
}