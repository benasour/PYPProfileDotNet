using System.ComponentModel.DataAnnotations;

namespace PYPProfileDotNet.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string Name { get; set; }
    }
}