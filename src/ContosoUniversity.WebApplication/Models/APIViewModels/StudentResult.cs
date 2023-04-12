using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class StudentResult : PageableResult
    {

        public List<Student> Students { get; set; }
    }
}