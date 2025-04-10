using DataServices.Data;
using DefaultValue.ApiRoute;
using DongTa.ResponseResult;
using DataTranfer.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
public class RewardsController(BhxhDbContext context) : ControllerBase {

    // GET: api/Rewards
    [HttpGet(ActionBase.All)]
    public async Task<IActionResult> All()
    {
        return Ok(ResultExtension.GetResult(
            await context.Rewards.Select(x => x.ToDto())
            .ToListAsync()));
    }

    // GET: Rewards/GetOne/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var reward = await context.Rewards.FindAsync(id);
        return Ok(ResultExtension.GetResult(reward?.ToDto()));
    }
}