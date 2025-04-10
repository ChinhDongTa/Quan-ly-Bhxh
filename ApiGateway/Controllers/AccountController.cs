using DataServices.Entities.Human;
using DataTranfer.Dtos;
using DongTa.BaseDapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DongTa.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using DefaultValue;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DongTa.TypeExtension;
using DefaultValue.ApiRoute;

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
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    RoleName = role.FirstOrDefault() ?? string.Empty
                };
                return Ok(ResultExtension.GetResult(result));
            }

            return NotFound("User not found");
        }

        [HttpGet(ActionBase.GetOne + "/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOne(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound));
            }
            var role = await userManager.GetRolesAsync(user);
            var result = new ApiUserDto(user.Id, user.UserName ?? string.Empty, string.Join(", ", role), user.EmployeeId);

            return Ok(ResultExtension.GetResult(result));
        }

        [HttpPost("AddRoleToUser")]
        [Authorize]
        public async Task<IActionResult> AddRoleToUser([FromBody] UserIdRoleName dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound));
            }
            var result = await userManager.AddToRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure("RoleIdkhoong hợp lệ"));
        }

        [HttpPost("RemoveRoleFromUser")]
        [Authorize]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserIdRoleName dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound));
            }
            var result = await userManager.RemoveFromRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure("RoleId không hợp lệ"));
        }

        [HttpDelete(ActionBase.Delete + "/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound));
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure("Id không hợp lệ"));
        }

        [HttpGet("GetRoles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await dapper.GetByQueryAsync<RoleDto>("Select Id, Name from AspNetRoles");
            return Ok(ResultExtension.GetResult(roles));
        }

        [HttpGet("GetAllRoleName")]
        [Authorize]
        public async Task<IActionResult> GetAllRoleName()
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<string>("Select Name from AspNetRoles")));
        }

        [HttpGet(ActionBase.All)]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var users = await userManager.Users.OrderBy(x => x.Email).ToListAsync();
            var list = new List<ApiUserDto>();
            foreach (var item in users)
            {
                var dto = new ApiUserDto(item.Id, item.UserName ?? string.Empty, string.Join(", ", await userManager.GetRolesAsync(item)), item.EmployeeId);
                list.Add(dto);
            }
            return Ok(ResultExtension.GetResult(list));
        }
    }
}