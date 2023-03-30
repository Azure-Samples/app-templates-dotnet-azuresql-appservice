using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.API.DTO
{
    public class StudentCourseResult
    {
        public int Count => Students.Count();
        public IList<Student> Students { get; set; }

        public StudentCourseResult()
        {
            Students = new List<Student>();
        }
    }
}