using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace PYPProfileDotNet.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string Name { get; set; }
    }

    public class GameOptions
    {
        public static List<SelectListItem> CoinSides
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Text = "Heads", Value = "Heads" },
                    new SelectListItem { Text = "Tails", Value = "Tails" }
                };
            }
        }

        public static List<SelectListItem> HorseSuits
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Text = "Spades", Value = "Spades" },
                    new SelectListItem { Text = "Hearts", Value = "Hearts" },
                    new SelectListItem { Text = "Clubs", Value = "Clubs" },
                    new SelectListItem { Text = "Diamonds", Value = "Diamonds" }
                };
            }
        }
    }
}