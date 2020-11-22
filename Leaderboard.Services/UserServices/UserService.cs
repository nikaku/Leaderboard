using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System.Collections.Generic;

namespace Leaderboard.Services.UserServices
{
    public class UserService : IUserService
    {
        private IUserRepository _userScoreRepository;
        public UserService(IUserRepository userScoreRepository)
        {
            _userScoreRepository = userScoreRepository;
        }

        public int Create(CreateUserDto createUserDto)
        {
            return _userScoreRepository.Add(new User { Username = createUserDto.Username });
        }

        public void Delete(int id)
        {
            _userScoreRepository.Delete(id);
        }

        public User Get(int id)
        {
            return _userScoreRepository.Get(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userScoreRepository.GetAll();
        }

        public User GetUserInfo(string username)
        {
            return _userScoreRepository.GetByUsername(username);
        }

        public User GetByUsername(string username)
        {
            return _userScoreRepository.GetByUsername(username);
        }


        public bool Update(User entity)
        {
            return _userScoreRepository.Update(entity);
        }
    }
}
