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
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityAPIContext _context;

        public CoursesController(ContosoUniversityAPIContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public IActionResult GetCourses()
        {
            //teste 01

            var courses = _context.Courses
                .Include(c => c.Department);

            //Transform to DTO
            var result = new DTO.CourseStudentResult()
            {
                Courses = courses.Select(c => new DTO.Course()
                {
                    ID = c.ID,
                    Credits = c.Credits,
                    Title = c.Title,
                    Department = new DTO.Department() {
                        ID = c.Department.ID,
                        Name = c.Department.Name,
                        Budget = c.Department.Budget,
                        StartDate = c.Department.StartDate
                    }
                }).ToList()
            };

            return Ok(result);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            //Transform to DTO
            var result = new DTO.Course()
            {
                ID = course.ID,
                Credits = course.Credits,
                Title = course.Title,
                Department = new DTO.Department()
                {
                    ID = course.Department.ID,
                    Name = course.Department.Name,
                    Budget = course.Department.Budget,
                    StartDate = course.Department.StartDate
                }
            };

            return Ok(result);
        }

        [HttpGet("CancelarMatricula/{id}")]
        public IActionResult CancelarMatricula([FromRoute] int id)
        {
            return Ok();
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse([FromRoute] int id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.ID)
            {
                return BadRequest();
            }

            course.Department = _context.Departments.Find(course.Department.ID);
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> PostCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            course.Department = _context.Departments.Find(course.Department.ID);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.ID }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }
    }
}