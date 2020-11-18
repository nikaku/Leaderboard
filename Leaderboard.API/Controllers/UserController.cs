using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Interfaces;
using Leaderboard.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public UserController(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = _userService.GetAll();
            if (user == null)
            {
                NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Register( CreateUserDto userDto)
        {
            try
            {
                var id = _userService.Create(userDto);
                return CreatedAtAction(nameof(Get), new { id }, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update(UpdateUserDto entity)
        {
            var res = _userService.Update(new BL.Entities.User
            {
                Id = entity.Id,
                Username = entity.Username
            });
            return Ok(res);
        }


    }
}
