using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Entities
{
    public class UserScore
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
