﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.API.ViewModels
{
    public class StudentViewModel
    {

        public int ID { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public byte[] Photo { get; set; }
    }
}