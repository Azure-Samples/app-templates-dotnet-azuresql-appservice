using Bogus;
using ContosoUniversity.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.API.Data
{
    public class DbInitializer
    {
        public static void Initialize(ContosoUniversityAPIContext context)
        {
            var random = new Random();
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Student.Any())
            {
                return;   // DB has been seeded
            }

            var instructorFaker = new Faker<Instructor>()
                .RuleFor(i => i.FirstName, f => f.Name.FirstName())
                .RuleFor(i => i.LastName, f => f.Name.LastName())
                .RuleFor(i => i.HireDate, f => f.Date.Past());

            var instructors = instructorFaker.Generate(1000);

            context.Instructors.AddRange(instructors);

            context.SaveChanges();

            var departments = new Department[]
            {
                new Department { Name = "English", Budget = 350000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Economics", Budget = 100000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] }
            };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();


            var courses = new Course[]
            {
                new Course {Title = "Chemistry",  Credits = 3, Department = departments.Single( s => s.Name == "Engineering") },
                new Course {Title = "Microeconomics", Credits = 3, Department = departments.Single( s => s.Name == "Economics") },
                new Course {Title = "Calculus", Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {Title = "Trigonometry", Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {Title = "Composition", Credits = 3, Department = departments.Single( s => s.Name == "English") },
                new Course {Title = "Literature", Credits = 4, Department = departments.Single( s => s.Name == "English") },
            };

            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var studentFaker = new Faker<Student>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Past());

            var students = studentFaker.Generate(10000);

            context.Student.AddRange(students);
            context.SaveChanges();

            var studentCourse = new List<StudentCourse>();

            for (int i = 1; i <= 10000; i++)
            {
                studentCourse.Add(
                    new StudentCourse
                    {
                        StudentID = i,
                        CourseID = random.Next(courses.Length)
                    });
            }

            context.StudentCourse.AddRange(studentCourse);

            context.SaveChanges();
        }

    }
}