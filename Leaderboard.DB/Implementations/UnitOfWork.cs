using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;

namespace Leaderboard.DB.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IUserScoreRepository UserScoreRepository { get; }

        public UnitOfWork(IUserRepository userRepository, IUserScoreRepository userScoreRepository)
        {
            UserRepository = userRepository;
            UserScoreRepository = userScoreRepository;
        }

        public void Dispose()
        {

        }

        public void SaveChanges()
        {

        }
    }
}
