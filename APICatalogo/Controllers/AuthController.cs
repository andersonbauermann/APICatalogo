using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APICatalogo.DTOs.UserLogin;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var token = _tokenService.GenerateToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var refreshTokenValidityInMinutes);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.UserName!);
        if (userExists is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new Response {Status = "Error", Message = "User already exists"});
        }

        User user = new()
        {
            Email = model.Email!,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName!
        };
        
        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new Response {Status = "Error", Message = "User creation failed."});
        }
        
        return Ok(new Response {Status = "Success", Message = "User created successfully!"});
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel token)
    {
        if (token is null) return BadRequest("Invalid token");
        
        var accessToken = token.AccessToken ?? throw new ArgumentNullException(nameof(token));
        var refreshToken = token.RefreshToken ?? throw new ArgumentNullException(nameof(token));
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, _configuration);
        
        if (principal is null) return BadRequest("Invalid access token");
        
        var userName = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(userName!);

        if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow || user.RefreshToken != refreshToken)
        {
            return BadRequest("Invalid refresh token");
        }

        var newAccessToken = _tokenService.GenerateToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{userName}")]
    public async Task<IActionResult> Revoke(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        
        if (user is null) return BadRequest("Invalid user name");
        
        user.RefreshToken = null;
        
        await _userManager.UpdateAsync(user);
        
        return NoContent();
    }
}