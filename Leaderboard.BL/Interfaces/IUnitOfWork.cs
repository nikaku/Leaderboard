using Leaderboard.BL.Interfaces.Repositories;

namespace Leaderboard.BL.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        void SaveChanges();
        void Dispose();
    }
}
