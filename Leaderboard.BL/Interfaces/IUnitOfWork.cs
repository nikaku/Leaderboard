using Leaderboard.BL.Interfaces.Repositories;
using System.Data;

namespace Leaderboard.BL.Interfaces
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}
