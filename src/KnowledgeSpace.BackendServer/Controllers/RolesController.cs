using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace KnowledgeSpace.BackendServer.Controllers
{
    public class RolesController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //URL: POST http://localhost:5000/api/roles
        [HttpPost]
        public async Task<IActionResult> PostRole(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            var role = new IdentityRole()
            {
                Id = request.Id,
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = role.Id }, request);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        //Get all roles
        //URL: GET http://localhost:5000/api/roles/
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleViewModel()
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();

            return Ok(roles); //200
        }

        //Get all roles filter
        //URL: GET http://localhost:5000/api/roles/?filter={filter}&pageIndex=2&pageSize=10
        [HttpGet("filter")]
        public async Task<IActionResult> GetRolesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles; // chỉ cần truy cập mà ko lấy ra list nên ko await
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Id.Contains(filter) || x.Name.Contains(filter));
            var totalRecods = await query.CountAsync();// count all records in Roles
            var items = await query.Skip((pageIndex - 1) * pageSize) // lấy trang thứ 2 , bắt đầu từ 11, thì skip 10 record đầu(2-1)*10=10
                .Take(pageSize)
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToListAsync();

            var pagination = new Pagination<RoleViewModel>()
            {
                Items = items,
                TotalRecords = totalRecods
            };
            return Ok(pagination); //200
        }

        //URL: GET http://localhost:5000/api/roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); //400
            }
            var request = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return Ok(request); //200
        }

        //URL: PUT http://localhost:5000/api/roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody] RoleCreateRequest request) // id lấy ở routing, còn roleVm lấy từ Body
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            if (id != request.Id)
            {
                return BadRequest();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); //400
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return NoContent(); //204 success without no content response 
            }
            return BadRequest(result.Errors);
        }

        //URL: DELETE http://localhost:5000/api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); //400
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                var roleVm = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(); //200
            }
            return BadRequest(result.Errors);
        }
    }
}
