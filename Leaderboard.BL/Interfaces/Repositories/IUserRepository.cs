using Leaderboard.BL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.BL.Interfaces.Repositories
{
    public interface IUserRepository
    {
        int Add(User entity);
        User Get(int id);
        User GetByUsername(string username);
        IEnumerable<User> GetAll();
        bool Update(User entity);
        void Delete(int id);
    }
}
