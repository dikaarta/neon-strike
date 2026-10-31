using Game.Web.Data;
using Game.Web.DTOs.Auth;
using Game.Web.Models;
using Game.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly GameDbContext _dbContext;
    private readonly PasswordService _passwordService;

    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        GameDbContext dbContext,
        PasswordService passwordService,
        JwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var usernameExists = await _dbContext.Users
            .AnyAsync(x => x.Username == request.Username);

        if (usernameExists)
        {
            return BadRequest("Username already exists.");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordService.HashPassword(
                request.Password),
            Coins = 0,
            BestScore = 0
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
    LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x =>
                x.Username == request.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var isValidPassword =
            _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash);

        if (!isValidPassword)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Token = token
        });
    }
}