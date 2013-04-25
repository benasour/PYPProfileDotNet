using System;
using System.ComponentModel.DataAnnotations;

namespace PYPProfileDotNet.Models
{
    public class History
    {
        [Key]
        public int id { get; set; }
        public Game Game { get; set; }
        public User User { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }

    #region HistoryViewModels
    public class Leaderboard
    {
        public User User { get; set; }
        public int Score { get; set; }
    }
    #endregion

}