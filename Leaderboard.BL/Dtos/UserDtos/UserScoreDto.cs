using System;

namespace Leaderboard.BL.Dtos.UserDtos
{
    public class UserScoreDto
    {
        public int UserId { get; set; }
        public int Score { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ScoreDate { get; set; }
    }
}
