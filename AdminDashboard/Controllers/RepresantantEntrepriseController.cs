using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RepresantantEntrepriseController : ControllerBase
{
    private readonly IRepresantantEntreprise _represantantEntreprise;

    public RepresantantEntrepriseController(IRepresantantEntreprise represantantEntreprise)
    {
        _represantantEntreprise = represantantEntreprise;
    }
    [Route("[action]")]
    [HttpGet]
    public async Task<ActionResult<List<RepresentantEntreprise>>> GetAll()
    {
        var represantants = await _represantantEntreprise.GetAll();
        return Ok(represantants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RepresentantEntreprise>> GetById(long id)
    {
        var represantant = await _represantantEntreprise.GetById(id);
        if (represantant == null)
        {
            return NotFound();
        }
        return Ok(represantant);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<RepresentantEntreprise>> GetByEmail(string email)
    {
        var represantant = await _represantantEntreprise.GetByEmail(email);
        if (represantant == null)
        {
            return NotFound();
        }
        return Ok(represantant);
    }
    [Route("[action]")]
    [HttpPost]
    public async Task<ActionResult<RepresentantEntreprise>> Add([FromForm]RepresentantEntreprise representantEntreprise)
    {
        var addedRepresantant = await _represantantEntreprise.Add(representantEntreprise);
        return CreatedAtAction(nameof(GetById), new { id = addedRepresantant.Id }, addedRepresantant);
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<RepresentantEntreprise>> Update(long id, [FromForm] RepresentantEntreprise representantEntreprise)
    {
        if (id != representantEntreprise.Id)
        {
            return BadRequest();
        }
        var updatedRepresantant = await _represantantEntreprise.Update(representantEntreprise);
        if (updatedRepresantant == null)
        {
            return NotFound();
        }
        return Ok(updatedRepresantant);
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult<RepresentantEntreprise>> RemoveById(long id)
    {
        var removedRepresantant = await _represantantEntreprise.RemoveById(id);
        if (removedRepresantant == null)
        {
            return NotFound();
        }
        return Ok(removedRepresantant);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<RepresentantEntreprise>> Authentification([FromQuery]string email,[FromQuery] string password)
    {
        var represantant = await _represantantEntreprise.Authentificate(email, password);
        if (represantant == null)
        {
            return Unauthorized();
        }
        return Ok(represantant);
    }
}