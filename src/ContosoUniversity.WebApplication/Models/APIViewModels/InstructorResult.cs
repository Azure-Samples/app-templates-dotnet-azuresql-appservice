using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class InstructorResult
    {
        public int Count { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
