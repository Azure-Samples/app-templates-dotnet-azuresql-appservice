using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class CoursesResult
    {
        public int Count { get; set; }
        public List<Course> Courses { get; set; }
    }
}