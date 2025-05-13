using DataServices.Entities.Human;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.BaseDapper;
using DongTa.ResponseResult;
using Dtos.Human;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Controllers {

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase {
        private readonly UserManager<ApiUser> userManager;
        private readonly IGenericDapper dapper;

        public AccountController(UserManager<ApiUser> _userManager, IGenericDapper _dapper)
        {
            userManager = _userManager;
            dapper = _dapper;
            dapper.DbNameType = DatabaseNameType.Employee;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
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
                    var addRoleresult = await userManager.AddToRoleAsync(user, "viewer");
                    if (!addRoleresult.Succeeded)
                    {
                        return BadRequest(addRoleresult.Errors);
                    }
                    return Ok(Result<bool>.Success(InfoMessage.ActionSuccess(CRUD.Create, ConstName.Registration)));
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Info")]
        public async Task<IActionResult> Info()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                //var role = await userManager.GetRolesAsync(user);
                var result = new InfoDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    RoleNames = await userManager.GetRolesAsync(user)
                };
                return Ok(ResultExtension.GetResult(result));
            }

            return Ok(Result<bool>.Failure("User không tồn tại"));
        }

        [HttpGet(ActionBase.GetOne + "/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound(nameof(ApiUser))));
            }
            var role = await userManager.GetRolesAsync(user);
            var result = new ApiUserDto(user.Id, user.UserName ?? string.Empty, string.Join(", ", role), user.EmployeeId);
            return Ok(ResultExtension.GetResult(result));
        }

        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromBody] UserIdRoleName dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound("RoleName")));
            }
            var result = await userManager.AddToRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId("RoleId")));
        }

        [HttpPost("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserIdRoleName dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound("User")));
            }
            var result = await userManager.RemoveFromRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId("RoleId")));
        }

        [HttpDelete(ActionBase.Delete + "/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound("User")));
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId("userId")));
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await dapper.GetByQueryAsync<RoleDto>("Select Id, Name from AspNetRoles");
            return Ok(ResultExtension.GetResult(roles));
        }

        [HttpGet("GetAllRoleName")]
        public async Task<IActionResult> GetAllRoleName()
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<string>("Select Name from AspNetRoles")));
        }

        [HttpGet(ActionBase.All)]
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