using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.API.Models
{
    /// <summary>
    /// Course
    /// </summary>
    public class Course
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Department Department { get; set; }

        public IList<StudentCourse> StudentCourse { get; set; }
    }
}