using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;

namespace Leaderboard.DB.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepository UserRepository { get; }

        public UnitOfWork(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public void Dispose()
        {

        }

        public void SaveChanges()
        {

        }
    }
}
