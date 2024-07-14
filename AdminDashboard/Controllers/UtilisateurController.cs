using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;
[Route("api/utilisateur")]
[ApiController]
public class UtilisateurController : ControllerBase
{
    private readonly IUtilisateur _utilisateur;
    public UtilisateurController(IUtilisateur utilisateur)
    {
        _utilisateur = utilisateur;
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
}