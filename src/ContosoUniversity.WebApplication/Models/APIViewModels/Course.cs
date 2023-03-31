﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    /// <summary>
    /// Course Class
    /// </summary>
    public class Course
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Department Department { get; set; }

    }
}