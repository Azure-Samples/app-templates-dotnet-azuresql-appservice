using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.API.DTO
{
    public class DepartamentInstructorResult
    {
        public int Count => Departments.Count();
        public IList<Department> Departments { get; set; }

        public DepartamentInstructorResult()
        {
            Departments = new List<Department>();
        }
    }
}