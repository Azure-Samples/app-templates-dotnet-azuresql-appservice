using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class StudentResult
    {
        public int Count { get; set; }
        public List<Student> Students { get; set; }
    }
}