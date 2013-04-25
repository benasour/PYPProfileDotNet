using System.ComponentModel.DataAnnotations;

namespace PYPProfileDotNet.Models
{
    public class FriendStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}