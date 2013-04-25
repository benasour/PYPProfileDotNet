using System.ComponentModel.DataAnnotations;

namespace PYPProfileDotNet.Models
{
    public class Friend
    {
        [Key]
        public int id { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public FriendStatus Status { get; set; }
    }
}