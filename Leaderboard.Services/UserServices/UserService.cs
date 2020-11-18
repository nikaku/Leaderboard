using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces;
using System;
using System.Collections.Generic;
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

        public string DownloadLeaderboardByDay(DateTime day)
        {
            throw new NotImplementedException();
        }

        public string DownloadLeaderboardByMonth(DateTime month)
        {
            throw new NotImplementedException();
        }

        public string DownloadLeaderboardByWeek(DateTime week)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            return _unitOfWork.UserRepository.Get(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll();
        }

        public string GetAllData()
        {
            throw new NotImplementedException();
        }

        public string GetLeaderboardByDay(DateTime day)
        {
            throw new NotImplementedException();
        }

        public string GetLeaderboardByMonth(DateTime month)
        {
            throw new NotImplementedException();
        }

        public string GetLeaderboardByWeek(DateTime week)
        {
            throw new NotImplementedException();
        }

        public string GetStats()
        {
            throw new NotImplementedException();
        }

        public User GetUserInfo(string username)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            return _unitOfWork.UserRepository.Update(entity);
        }

        public void UploadLeaderboardData(string filePath)
        {
            throw new NotImplementedException();
        }

    }
}
