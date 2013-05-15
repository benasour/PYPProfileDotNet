using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Data.Entity;

namespace PYPProfileDotNet.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<PYPContext>
    {
        protected override void Seed(PYPContext context)
        {
            string salt = Crypto.GenerateSalt();
            string password = Crypto.HashPassword("password" + salt);

            var users = new List<User>
            {
                new User { UserId = 1, UserName = "Andy", Password = password, Salt = salt, Name = "Andy", Email = "email@example.com" },
                new User { UserId = 2, UserName = "Ben", Password = password, Salt = salt, Name = "Ben", Email = "email@example.com" },
                new User { UserId = 3, UserName = "Paul", Password = password, Salt = salt, Name = "Paul", Email = "email@example.com" },
                new User { UserId = 4, UserName = "Karl", Password = password, Salt = salt, Name = "Karl", Email = "email@example.com" },
                new User { UserId = 5, UserName = "guest", Password = password, Salt = salt, Name = "guest", Email = "email@example.com" }
            };
            users.ForEach(u => context.Users.Add(u));

            var games = new List<Game>
            {
                new Game { GameId = 1, Name = "Horse" },
                new Game { GameId = 2, Name = "Coin" }
            };
            games.ForEach(g => context.Games.Add(g));

            var friendStatuses = new List<FriendStatus>
            {
                new FriendStatus { StatusId = 1, Status = "accepted" },
                new FriendStatus { StatusId = 2, Status = "defriended" },
                new FriendStatus { StatusId = 3, Status = "requested" },
                new FriendStatus { StatusId = 4, Status = "declined" },
                new FriendStatus { StatusId = 5, Status = "blocked" },
                new FriendStatus { StatusId = 6, Status = "blockedPending" }
            };
            friendStatuses.ForEach(s => context.FriendStatuses.Add(s));

            new List<Friend> 
            {
                new Friend { User1 = users.Single(u => u.UserId == 1), User2 = users.Single(u => u.UserId == 2), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Andy is friends with Ben
                new Friend { User1 = users.Single(u => u.UserId == 1), User2 = users.Single(u => u.UserId == 3), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Andy is friends with Paul
                new Friend { User1 = users.Single(u => u.UserId == 4), User2 = users.Single(u => u.UserId == 1), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Karl is friends with Andy
                new Friend { User1 = users.Single(u => u.UserId == 2), User2 = users.Single(u => u.UserId == 3), Status = friendStatuses.Single(s => s.StatusId == 3) }, // Ben has requested Paul
                new Friend { User1 = users.Single(u => u.UserId == 2), User2 = users.Single(u => u.UserId == 4), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Ben is friends with Karl
                new Friend { User1 = users.Single(u => u.UserId == 5), User2 = users.Single(u => u.UserId == 1), Status = friendStatuses.Single(s => s.StatusId == 2) }, // guest has defriended Andy
                new Friend { User1 = users.Single(u => u.UserId == 3), User2 = users.Single(u => u.UserId == 5), Status = friendStatuses.Single(s => s.StatusId == 4) }, // Paul has declined guest
                new Friend { User1 = users.Single(u => u.UserId == 3), User2 = users.Single(u => u.UserId == 2), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Paul is friends with Ben
                new Friend { User1 = users.Single(u => u.UserId == 2), User2 = users.Single(u => u.UserId == 5), Status = friendStatuses.Single(s => s.StatusId == 1) }, // Ben is friends with guest
                new Friend { User1 = users.Single(u => u.UserId == 4), User2 = users.Single(u => u.UserId == 5), Status = friendStatuses.Single(s => s.StatusId == 2) }  // Karl has defriended guest
            }.ForEach(f => context.Friends.Add(f));

            new List<History>
            {
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 21), Score = 20 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 21), Score = 10 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 21), Score = 8 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 21), Score = -1 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 21), Score = -3 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 21), Score = -5 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 22), Score = -7 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 22), Score = 12 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 22), Score = -12 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 22), Score = -2 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 22), Score = 13 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 22), Score = 7 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 8 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 2 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 3 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 5 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = -5 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 23), Score = -3 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = -4 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 23), Score = -6 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 23), Score = -2 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 23), Score = 2 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 3 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 23), Score = 4 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 4 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 25 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = -30 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 21 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = 6 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = 9 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = -4 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = -6 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 8 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = -3 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 5 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = 3 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = -4 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 24), Score = 5 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 10 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 17 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 24), Score = 2 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 4 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = -12 },
                new History { User = users.Single(u => u.UserId == 3), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = -3 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = 4 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = 8 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 9 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 11 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 11 },
                new History { User = users.Single(u => u.UserId == 1), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = -9 },
                new History { User = users.Single(u => u.UserId == 5), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = -5 },
                new History { User = users.Single(u => u.UserId == 4), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 15 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 1), Date = new DateTime(2013, 4, 25), Score = -15 },
                new History { User = users.Single(u => u.UserId == 2), Game = games.Single(g => g.GameId == 2), Date = new DateTime(2013, 4, 25), Score = 6 }
            }.ForEach(h => context.History.Add(h));

        }
    }
}