using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class InstructorResult
    {
        public int Count { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
