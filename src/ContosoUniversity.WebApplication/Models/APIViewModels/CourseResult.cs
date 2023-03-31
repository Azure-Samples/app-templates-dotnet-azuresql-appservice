using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class CoursesResult
    {
        public int Count { get; set; }
        public List<Course> Courses { get; set; }
    }
}