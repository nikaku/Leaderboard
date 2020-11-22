using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Data;

namespace Leaderboard.DB.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        IDbConnection _dbConnection;
        IDbTransaction _transaction;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        IDbConnection IUnitOfWork.Connection
        {
            get { return _dbConnection; }
        }

        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        public void Begin()
        {
            _dbConnection.Open();
            _transaction = _dbConnection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            _dbConnection.Close();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dbConnection.Close();
        }
    }
}
