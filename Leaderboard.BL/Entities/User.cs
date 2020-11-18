using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<UserScore> Scores { get; set; }
    }
}
