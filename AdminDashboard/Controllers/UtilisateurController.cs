using AdminDashboard.Data;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;
[Route("api/utilisateur")]
[ApiController]
public class UtilisateurController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public UtilisateurController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var utilisateurs = _context.Utilisateurs.ToList();
        return Ok(utilisateurs);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] long id)
    {
        var utilisateur =  _context.Utilisateurs.Find(id); 
        if (utilisateur == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(utilisateur);
        }
    }
}