using DataServices.Entities.Human;
using DataTranfer.Dtos;
using DongTa.BaseDapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DongTa.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using DefaultValue;

namespace ApiGateway.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApiUser> userManager, IGenericDapper dapper) : ControllerBase {

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = new ApiUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    EmployeeId = dto.EmployeeId,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    var addRoleresult = await userManager.AddToRoleAsync(user, dto.RoleId);
                    if (!addRoleresult.Succeeded)
                    {
                        return BadRequest(addRoleresult.Errors);
                    }
                    return Ok(Result<bool>.Success(InfoMessage.ActionSuccess(ConstName.Registration)));
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Info")]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                var result = new InfoDto
                {
                    Email = user.Email!,
                    Username = user.UserName!,
                    RoleName = role.FirstOrDefault() ?? string.Empty
                };
                return Ok(ResultExtension.GetResult(result));
            }

            return NotFound("User not found");
        }

        [HttpPost("AddRoleToUser")]
        [Authorize]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await userManager.AddToRoleAsync(user, dto.RoleId);
            if (result.Succeeded)
            {
                return Ok("Role added successfully");
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("GetRoles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await dapper.GetByQueryAsync<RoleDto>("Select Id, Name from AspNetRoles");
            return Ok(ResultExtension.GetResult(roles));
        }
    }
}