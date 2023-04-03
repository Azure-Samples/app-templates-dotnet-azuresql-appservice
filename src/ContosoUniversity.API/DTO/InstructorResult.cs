using System.Collections.Generic;
namespace ContosoUniversity.API.DTO
{
    public class InstructorResult : PageableResult
    {
        public IList<Instructor> Instructors { get; set; }

        public InstructorResult()
        {
            Instructors = new List<Instructor>();
        }
    }
}
