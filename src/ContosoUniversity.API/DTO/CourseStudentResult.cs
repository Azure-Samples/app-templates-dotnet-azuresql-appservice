using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.API.DTO
{
    public class CourseStudentResult
    {
        public int Count => Courses.Count();
        public IList<Course> Courses { get; set; }

        public CourseStudentResult()
        {
            Courses = new List<Course>();
        }
    }
}