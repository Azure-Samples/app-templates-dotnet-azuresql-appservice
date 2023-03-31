using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.API.DTO
{
    public class StudentCourseResult
    {
        public int Pages { get; set; }

        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public IList<Student> Students { get; set; }

        public StudentCourseResult()
        {
            Students = new List<Student>();
        }
    }
}