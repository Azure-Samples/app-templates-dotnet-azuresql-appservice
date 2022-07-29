using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.API.Data;
using ContosoUniversity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityAPIContext _context;

        public DepartmentsController(ContosoUniversityAPIContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departaments = _context.Departments
                                    .Include(i => i.Instructor);

            //Transform to DTO
            var result = new DTO.DepartamentInstructorResult()
            {
                Departments = departaments.Select(c => new DTO.Department()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Budget = c.Budget,
                    StartDate = c.StartDate,
                    Instructor = new DTO.Instructor()
                    {
                        ID = c.Instructor.ID,
                        LastName = c.Instructor.LastName,
                        FirstName = c.Instructor.FirstName,
                        HireDate = c.Instructor.HireDate
                    }
                }).ToList()
            };

            return Ok(result);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _context.Departments
                .Include(i => i.Instructor)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ID == id);

            if (department == null)
            {
                return NotFound();
            }

            //Transform to DTO
            var result = new DTO.Department()
            {
                ID = department.ID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Instructor = new DTO.Instructor()
                {
                    ID = department.Instructor.ID,
                    LastName = department.Instructor.LastName,
                    FirstName = department.Instructor.FirstName,
                    HireDate = department.Instructor.HireDate
                }
            };

            return Ok(result);
        }

        // PUT: api/Departments/5 - Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment([FromRoute] int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.ID)
            {
                return BadRequest();
            }

            department.Instructor = _context.Instructors.Find(department.Instructor.ID);
            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments - Create
        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            department.Instructor = _context.Instructors.Find(department.Instructor.ID);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.ID }, department);
        }

        // DELETE: api/Departments/5 - Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}