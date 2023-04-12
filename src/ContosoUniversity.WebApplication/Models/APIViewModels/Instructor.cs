﻿using System;
using System.ComponentModel;

namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class Instructor
    {
        public int ID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTime HireDate { get; set; }

        [DisplayName("Instructor Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}