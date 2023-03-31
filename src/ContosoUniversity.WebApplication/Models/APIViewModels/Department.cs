﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class Department
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        public Instructor Instructor { get; set; }
    }
}