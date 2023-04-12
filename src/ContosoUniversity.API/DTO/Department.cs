using System;

namespace ContosoUniversity.API.DTO
{
    public class Department
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public Instructor Instructor { get; set; }
    }
}