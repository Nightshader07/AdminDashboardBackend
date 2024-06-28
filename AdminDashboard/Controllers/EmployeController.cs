using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;
[Route("api/[Controller]")]
[ApiController]
public class EmployeController : ControllerBase
{
    private readonly IEmplyeRepository _emplyeRepository;

    public EmployeController(IEmplyeRepository emplyeRepository)
    {
        _emplyeRepository = emplyeRepository;
    }

    [Route("[action]")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_emplyeRepository.GetAll());
    }
    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        return Ok(_emplyeRepository.GetById(id));
    }
    [Route("[action]")]
    [HttpPost]
    public IActionResult Add([FromForm]Employe employe)
    {
        Employe? employee = _emplyeRepository.Add(employe);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }
    [Route("[action]")]
    [HttpPut]
    public IActionResult Update([FromForm]Employe employe)
    {
        Employe? employee = _emplyeRepository.Update(employe);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }
    [Route("[action]")]
    [HttpDelete]
    public IActionResult Remove(long id)
    {
        Employe? employee = _emplyeRepository.RemoveById(id);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }
    [HttpGet("[action]")]
    public IActionResult Authentification([FromQuery]string email,[FromQuery]string password)
    {
        var employe = _emplyeRepository.Authentificate(email, password);
        if (employe == null)
        {
            return Unauthorized(); 
        }

        return Ok(employe);
    }
}