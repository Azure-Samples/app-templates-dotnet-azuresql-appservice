using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.API.DTO
{
    public class InstructorResult
    {
        public int Count => Instructors.Count();
        public IList<Instructor> Instructors { get; set; }

        public InstructorResult()
        {
            Instructors = new List<Instructor>();
        }
    }
}
