using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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