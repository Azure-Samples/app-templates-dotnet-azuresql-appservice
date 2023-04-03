using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.API.Data;
using ContosoUniversity.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly ContosoUniversityAPIContext _context;

        public InstructorsController(ContosoUniversityAPIContext context)
        {
            _context = context;
        }

        // GET: api/Instructors
        [HttpGet]
        public async Task<IActionResult> GetInstructors(int? page)
        {
            var instructors = _context.Instructors;

            int pageNumber = page.HasValue && page.Value > 0 ? page.Value - 1 : 0; // 0-index
            int pageSize = 50;

            int totalInstructors = await instructors.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalInstructors / pageSize);

            //Transform to DTO
            var result = new DTO.InstructorResult()
            {
                Instructors = await instructors.OrderBy(s => s.ID).Skip(pageNumber * pageSize).Take(pageSize).
                Select(c => new DTO.Instructor()
                {
                    ID = c.ID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    HireDate = c.HireDate
                }).ToListAsync(),
                Count = totalInstructors,
                Pages = totalPages,
                CurrentPage = pageNumber + 1
            };

            return Ok(result);

        }

        // GET: api/Instructors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            //Transform to DTO
            var result = new DTO.Instructor()
            {
                ID = instructor.ID,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                HireDate = instructor.HireDate
            };

            return Ok(result);
        }

        // PUT: api/Instructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor([FromRoute] int id, [FromBody] Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instructor.ID)
            {
                return BadRequest();
            }

            _context.Entry(instructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Instructors
        [HttpPost]
        public async Task<IActionResult> PostInstructor([FromBody] Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstructor", new { id = instructor.ID }, instructor);
        }

        // DELETE: api/Instructors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return Ok(instructor);
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}