﻿using System;
using System.Collections.Generic;

namespace ContosoUniversity.API.DTO
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
