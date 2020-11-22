using Dapper;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Leaderboard.DB.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Add(User entity)
        {
            var sql = "INSERT INTO [dbo].[Users] (Username) OUTPUT Inserted.Id VALUES (@Username)";
            var res = _unitOfWork.Connection.QuerySingle<int>(sql, entity);
            return res;
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM [dbo].[Users] WHERE Id = @id";
            var res = _unitOfWork.Connection.Execute(sql, new { Id = id });
        }

        public User Get(int id)
        {
            var sql = "SELECT * FROM [dbo].[Users] WHERE Id = @id";
            var res = _unitOfWork.Connection.Query<User>(sql, new { Id = id }).SingleOrDefault();
            return res;
        }
        public IEnumerable<User> GetAll()
        {
            var sql = "SELECT * FROM [dbo].[Users]";
            var res = _unitOfWork.Connection.Query<User>(sql);
            return res;
        }

        public User GetByUsername(string username)
        {
            var sql = "SELECT * FROM [dbo].[Users] WHERE username = @username";
            var res = _unitOfWork.Connection.Query<User>(sql, new { username }).SingleOrDefault();
            return res;
        }

        public bool Update(User entity)
        {
            var sql = "update [dbo].[Users] set Username = @Username where Id = @Id";
            var res = _unitOfWork.Connection.Execute(sql, new { Id = entity.Id, Username = entity.Username });
            return res == 1;
        }
    }
}
