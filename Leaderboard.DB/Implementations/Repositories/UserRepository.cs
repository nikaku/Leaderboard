using Dapper;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard.DB.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public int Add(User entity)
        {
            var sql = "INSERT INTO [dbo].[Users] (Username) OUTPUT Inserted.Id VALUES (@Username)";
            return _dbConnection.QuerySingle<int>(sql, entity);
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM [dbo].[Users] WHERE Id = @id";
            var res = _dbConnection.Execute(sql, new { Id = id });
        }

        public User Get(int id)
        {
            var sql = "SELECT * FROM [dbo].[Users] WHERE Id = @id";
            var res = _dbConnection.Query<User>(sql, new { Id = id }).SingleOrDefault();
            return res;
        }
        public IEnumerable<User> GetAll()
        {
            var sql = "SELECT * FROM [dbo].[Users]";
            var res = _dbConnection.Query<User>(sql);
            return res;
        }
        public bool Update(User entity)
        {
            var sql = "update [dbo].[Users] set Username = @Username where Id = @Id";
            var res = _dbConnection.Execute(sql, new { Id = entity.Id, Username = entity.Username });
            return res == 1;
        }
    }
}
