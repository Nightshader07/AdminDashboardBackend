using AdminDashboard.Data;
using AdminDashboard.DTOs;
using AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdminDashboard.Controllers;
[Route("api/utilisateur")]
[ApiController]
public class UtilisateurController : ControllerBase
{
    private readonly IUtilisateur _utilisateur;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    public UtilisateurController(IUtilisateur utilisateur, IJwtService jwtService, IRefreshTokenService refreshTokenService)
    {
        _utilisateur = utilisateur;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        return Ok(_utilisateur.GetAll());
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] long id)
    {
        var utilisateur =  _utilisateur.GetById(id); 
        if (utilisateur == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(utilisateur);
        }
    }
    [HttpGet("[action]")]
    public IActionResult Authentification([FromQuery]string email, [FromQuery]string password)
    {
        var utilisateur = _utilisateur.Authentificate(email, password);
        if (utilisateur == null)
        {
            return Unauthorized(); 
        }

        return Ok(utilisateur);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = _utilisateur.Authentificate(loginDto.Email, loginDto.Password);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = _jwtService.GenerateJwtToken(user.Id.ToString());
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id.ToString());

        return Ok(new
        {
            Token = token,
            RefreshToken = refreshToken.Token
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var response = await _utilisateur.RefreshTokenAsync(refreshTokenDto.Token, refreshTokenDto.RefreshToken);
            return Ok(response);
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    [HttpPost("[action]")]
    public IActionResult Authentificate([FromBody] String refreshToken)
    {
        try
        {
            var response = _utilisateur.IsTokenValid(refreshToken);
            return Ok(response);
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        bool result = await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
    
        if (!result)
        {
            return BadRequest("Invalid or already revoked token.");
        }

        return Ok(new { message = "Logged out successfully." });
    }
}