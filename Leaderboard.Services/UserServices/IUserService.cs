using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.Services.UserServices
{
    public interface IUserService
    {
        public User GetUserInfo(string username);
        public int Create(CreateUserDto createUserDto);
        public User Get(int id);
        public User GetByUsername(string id);
        public void Delete(int id);
        public IEnumerable<User> GetAll();
        public bool Update(User entity);

    }
}
