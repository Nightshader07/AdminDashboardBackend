using AdminDashboard.DTOs;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TacheController : ControllerBase
{
    private readonly ITache _tacheRepository;
    private readonly IUtilisateur _utilisateur;
    private readonly IColumn _column;


    public TacheController(ITache tache, IUtilisateur utilisateur, IColumn column)
    {
        _tacheRepository = tache;
        _utilisateur = utilisateur;
        _column = column;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var taches = _tacheRepository.GetAll();
        return Ok(taches);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        var tache = _tacheRepository.GetById(id);
        if (tache == null)
        {
            return NotFound();
        }
        return Ok(tache);
    }

    [HttpPost("[action]")]
    public IActionResult Add([FromBody] TacheDto tacheDto)
        {
            if (tacheDto == null)
            {
                return BadRequest();
            }

            var utilisateur = _utilisateur.GetById(tacheDto.UtilisateurId);
            if (utilisateur == null)
            {
                return BadRequest("UtilisateurId is invalid.");
            }

            var column = _column.GetById(tacheDto.ColumnId);
            if (column == null)
            {
                return BadRequest("ColumnId is invalid.");
            }

            var tache = new Tache
            {
                Name = tacheDto.Name,
                Deadline = tacheDto.Deadline,
                utilisateurId = tacheDto.UtilisateurId,
                Utilisateur = utilisateur,
                ColumnId = tacheDto.ColumnId,
                Column = column
            };

            var createdTache = _tacheRepository.Add(tache);
            return CreatedAtAction(nameof(GetById), new { id = createdTache.Id }, new TacheDto
            {
                Id = createdTache.Id,
                Name = createdTache.Name,
                Deadline = createdTache.Deadline,
                UtilisateurId = createdTache.utilisateurId,
                ColumnId = createdTache.ColumnId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TacheDto tacheDto)
        {
            if (tacheDto == null || tacheDto.Id != id)
            {
                return BadRequest();
            }

            var existingTache = _tacheRepository.GetById(id);
            if (existingTache == null)
            {
                return NotFound();
            }

            var utilisateur = _utilisateur.GetById(tacheDto.UtilisateurId);
            if (utilisateur == null)
            {
                return BadRequest("UtilisateurId is invalid.");
            }

            var column = _column.GetById(tacheDto.ColumnId);
            if (column == null)
            {
                return BadRequest("ColumnId is invalid.");
            }

            existingTache.Name = tacheDto.Name;
            existingTache.Deadline = tacheDto.Deadline;
            existingTache.utilisateurId = tacheDto.UtilisateurId;
            existingTache.Utilisateur = utilisateur;
            existingTache.ColumnId = tacheDto.ColumnId;
            existingTache.Column = column;

            _tacheRepository.Update(existingTache);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveById(long id)
        {
            var tache = _tacheRepository.GetById(id);
            if (tache == null)
            {
                return NotFound();
            }

            _tacheRepository.RemoveById(id);
            return NoContent();
        }
}