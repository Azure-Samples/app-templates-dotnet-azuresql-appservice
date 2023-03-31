using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.API.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ContosoUniversityAPIContext _context;
        
        public StudentsController(ContosoUniversityAPIContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetStudent(int? page)
        {
            var students = _context.Student
                .Include(s => s.StudentCourse)
                .ThenInclude(s => s.Course)
                .AsNoTracking();

            int pageNumber = page.HasValue && page.Value > 0 ? page.Value - 1 : 0; // 0-index
            int pageSize = 50;

            int totalStudents = await students.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            //Transform to DTO
            var result = new DTO.StudentCourseResult()
            {
                Students = await students.OrderBy(s => s.ID).Skip(pageNumber * pageSize).Take(pageSize).
                Select(s => new DTO.Student()
                    {
                        ID = s.ID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Courses = s.StudentCourse.Select(c => new DTO.Course()
                        {
                            ID = c.Course.ID,
                            Title = c.Course.Title,
                            Credits = c.Course.Credits
                        }).ToList(),
                        EnrollmentDate = s.EnrollmentDate
                    }).
                ToListAsync(),
                Count = totalStudents,
                Pages = totalPages,
                CurrentPage = pageNumber + 1
            };

            return Ok(result);
            
        }

        // GET: api/Students/Search?name=Ana
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var students = await _context.Student
                .Include(s => s.StudentCourse)
                .ThenInclude(s => s.Course)
                .AsNoTracking()
                .Where(s => EF.Functions.Like(s.FirstName, name+"%") || EF.Functions.Like(s.LastName, name + "%"))
                .ToListAsync();
            
            if (students == null)
            {
                return NotFound();
            }

            //Transform to DTO
            var result = new DTO.StudentCourseResult()
            {
                Students = students.Select(s => new DTO.Student()
                {
                    ID = s.ID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Courses = s.StudentCourse.Select(c => new DTO.Course()
                    {
                        ID = c.Course.ID,
                        Title = c.Course.Title,
                        Credits = c.Course.Credits
                    }).ToList(),
                    EnrollmentDate = s.EnrollmentDate
                }).ToList()
            };

            return Ok(result);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Student
                .Include(s => s.StudentCourse)
                .ThenInclude(s => s.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }
            
            //Transform to DTO
            var result = new DTO.Student()
            {
                ID = student.ID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Courses = student.StudentCourse.Select(c => new DTO.Course()
                {
                    ID = c.Course.ID,
                    Title = c.Course.Title,
                    Credits = c.Course.Credits
                }).ToList(),
                EnrollmentDate = student.EnrollmentDate
            };

            return Ok(result);
        }

        // PUT: api/Students/5 - Alter
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] Models.Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students - Create
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Models.Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                _context.Student.Add(student);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStudent", new { id = student.ID }, student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Students/5 - Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}