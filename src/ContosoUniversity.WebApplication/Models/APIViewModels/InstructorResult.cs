using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class InstructorResult : PageableResult
    {
        public List<Instructor> Instructors { get; set; }
    }
}
