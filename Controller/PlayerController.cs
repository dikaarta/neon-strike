using System.Security.Claims;
using Game.Web.Data;
using Game.Web.DTOs.Player;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Web.Controllers;

[ApiController]
[Route("api/player")]
[Authorize]
public class PlayerController : ControllerBase
{
    private readonly GameDbContext _dbContext;

    public PlayerController(GameDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<PlayerProfileResponse>> GetProfile()
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = int.Parse(userIdClaim.Value);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        return new PlayerProfileResponse
        {
            Username = user.Username,
            Coins = user.Coins,
            BestScore = user.BestScore,
            SelectedAvatarId = user.SelectedAvatarId
        };
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveProgress(
    SaveProgressRequest request)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = int.Parse(userIdClaim.Value);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        user.Coins = request.Coins;
        user.BestScore = request.BestScore;
        user.SelectedAvatarId = request.SelectedAvatarId;

        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}