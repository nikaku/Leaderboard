using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Dtos.UserDtos
{
    public class UserRating
    {
        public string Username { get; set; }
        public int Rating { get; set; }
        public int Score { get; set; }
    }
}
