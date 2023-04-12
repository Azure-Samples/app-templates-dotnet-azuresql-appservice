using Bogus;
using ContosoUniversity.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.API.Data
{
    public class DbInitializer
    {
        public async static Task Initialize(ContosoUniversityAPIContext context)
        {
            var random = new Random();
            await context.Database.EnsureCreatedAsync();

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

            await context.Instructors.AddRangeAsync(instructors);

            await context.SaveChangesAsync();

            var departments = new Department[]
            {
                new Department { Name = "English", Budget = 350000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] },
                new Department { Name = "Economics", Budget = 100000, StartDate = DateTime.Parse("01/09/2007"), Instructor  = instructors[random.Next(instructors.Count)] }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();


            var courses = new Course[]
            {
                new Course {Title = "Chemistry",  Credits = 3, Department = departments.Single( s => s.Name == "Engineering") },
                new Course {Title = "Microeconomics", Credits = 3, Department = departments.Single( s => s.Name == "Economics") },
                new Course {Title = "Calculus", Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {Title = "Trigonometry", Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {Title = "Composition", Credits = 3, Department = departments.Single( s => s.Name == "English") },
                new Course {Title = "Literature", Credits = 4, Department = departments.Single( s => s.Name == "English") },
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            var studentFaker = new Faker<Student>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Past());

            var students = studentFaker.Generate(10000);

            await context.Student.AddRangeAsync(students);
            await context.SaveChangesAsync();

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

            await context.StudentCourse.AddRangeAsync(studentCourse);

            await context.SaveChangesAsync();
        }

    }
}