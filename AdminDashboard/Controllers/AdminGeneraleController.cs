using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AdminGeneraleController : ControllerBase
    {
        private readonly IAdminGenerale _adminGenerale;

        public AdminGeneraleController(IAdminGenerale adminGenerale)
        {
            _adminGenerale = adminGenerale;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_adminGenerale.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            return Ok(_adminGenerale.GetById(id));
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Add([FromForm]AdminGenerale adminGenerale)
        {
            AdminGenerale? newAdminGenerale = _adminGenerale.Add(adminGenerale);
            if (newAdminGenerale == null)
            {
                return BadRequest();
            }
            return Ok(newAdminGenerale);
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult Update([FromForm]AdminGenerale adminGenerale)
        {
            AdminGenerale? updatedAdminGenerale = _adminGenerale.Update(adminGenerale);
            if (updatedAdminGenerale == null)
            {
                return BadRequest();
            }
            return Ok(updatedAdminGenerale);
        }

        [Route("[action]")]
        [HttpDelete]
        public IActionResult Remove(long id)
        {
            AdminGenerale? removedAdminGenerale = _adminGenerale.RemoveById(id);
            if (removedAdminGenerale == null)
            {
                return BadRequest();
            }
            return Ok(removedAdminGenerale);
        }

        [HttpGet("[action]")]
        public IActionResult Authentification([FromQuery]string email, [FromQuery]string password)
        {
            var adminGenerale = _adminGenerale.Authentificate(email, password);
            if (adminGenerale == null)
            {
                return Unauthorized(); 
            }

            return Ok(adminGenerale);
        }
    }
}
