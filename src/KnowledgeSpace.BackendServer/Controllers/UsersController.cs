using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreateRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email,
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, request);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;

            var usersVm =await users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Dob = u.Dob,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync();

            return Ok(usersVm);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var request =await _userManager.FindByIdAsync(Id);
            if(request == null)
            {
                return NotFound();
            }
            var userVm = new UserViewModel()
            {
                Id = request.Id,
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber
            };

            return Ok(userVm);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetUsersPaging(string filter,int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (!String.IsNullOrEmpty(filter))
                query = query.Where(q => q.Email.Contains(filter)
                   || q.Email.Contains(filter)
                   || q.Email.Contains(filter));

            var totalRecords =await query.CountAsync();
            var items =await query.Skip((pageIndex-1)*pageSize)
                .Take(pageSize)
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Dob = u.Dob,
                    PhoneNumber = u.PhoneNumber
                }).ToListAsync();
            var pagination = new Pagination<UserViewModel>()
            {
                Items = items,
                TotalRecords = totalRecords
            };
            return Ok(pagination);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string Id, [FromBody]UserCreateRequest request)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if(user == null)
                return NotFound();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob = request.Dob;

            var result =await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return NoContent();
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user =await _userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();
            var result =await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var userVm = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Dob = user.Dob,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }
}
