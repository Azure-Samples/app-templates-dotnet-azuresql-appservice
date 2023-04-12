using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class DepartmentResult
    {
        public int Count { get; set; }
        public List<Department> Departments { get; set; }
    }
}
