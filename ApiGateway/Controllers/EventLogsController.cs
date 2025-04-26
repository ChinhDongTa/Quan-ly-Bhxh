using DefaultValue;
using DongTa.BaseDapper;
using DongTa.ResponseResult;
using Dtos.Human;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class EventLogsController(IGenericDapper dapper) : ControllerBase {

        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<EventLogDto>(
                SqlQueryString.SelectEventLogDtoByEmployeeId(employeeId))));
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<EventLogDto>(
                SqlQueryString.SelectEventLogDtoByUserId(userId))));
        }

        [HttpGet("GetByUserName/{email}")]
        public async Task<IActionResult> GetByUserName(string email)
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<EventLogDto>(
                SqlQueryString.SelectEventLogDtoByUserName(email))));
        }

        [HttpGet("GetTop/{top}")]
        public async Task<IActionResult> GetTop(int top = 15)
        {
            return Ok(ResultExtension.GetResult(await dapper.GetByQueryAsync<EventLogDto>(
                SqlQueryString.SelectTopEventLogDto(top))));
        }
    }
}