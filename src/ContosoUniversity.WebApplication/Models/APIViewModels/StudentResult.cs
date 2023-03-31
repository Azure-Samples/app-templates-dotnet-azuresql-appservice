using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class StudentResult
    {
        public int Count { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
        public List<Student> Students { get; set; }
    }
}