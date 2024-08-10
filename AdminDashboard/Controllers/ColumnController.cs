using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ColumnController : ControllerBase
{
        private readonly IColumn _columnRepository;

        public ColumnController(IColumn columnRepository)
        {
            _columnRepository = columnRepository;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Column>> GetAll()
        {
            var columns = _columnRepository.GetAll();
            return Ok(columns);
        }

        [HttpGet("{id}")]
        public ActionResult<Column> GetById(long id)
        {
            var column = _columnRepository.GetById(id);
            if (column == null)
            {
                return NotFound();
            }
            return Ok(column);
        }

        [HttpPost("[action]")]
        public ActionResult<Column> Add(Column column)
        {
            var addedColumn = _columnRepository.Add(column);
            return CreatedAtAction(nameof(GetById), new { id = addedColumn.Id }, addedColumn);
        }

        [HttpPut("[action]")]
        public ActionResult<Column> Update(long id, Column column)
        {
            if (id != column.Id)
            {
                return BadRequest();
            }
            var updatedColumn = _columnRepository.Update(column);
            return Ok(updatedColumn);
        }

        [HttpDelete("[action]")]
        public ActionResult<Column> RemoveById(long id)
        {
            var column = _columnRepository.GetById(id);
            if (column == null)
            {
                return NotFound();
            }
            _columnRepository.RemoveById(id);
            return Ok(column);
        }
    }
