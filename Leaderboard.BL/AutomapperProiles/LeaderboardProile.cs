using AutoMapper;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.AutomapperProiles
{
    public class LeaderboardProile : Profile
    {
        public LeaderboardProile()
        {
            CreateMap<UserScore, LeaderboardDto>();
        }
    }
}
