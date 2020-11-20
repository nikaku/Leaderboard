using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaderboard.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Create(CreateUserDto createUserDto)
        {
            return _unitOfWork.UserRepository.Add(new User { Username = createUserDto.Username });
        }

        public void Delete(int id)
        {
            _unitOfWork.UserRepository.Delete(id);
        }

        public User Get(int id)
        {
            return _unitOfWork.UserRepository.Get(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll();
        }

        public User GetUserInfo(string username)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            return _unitOfWork.UserRepository.Update(entity);
        }
    }
}
