using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task9.Data;

namespace Task9.Models.TaskModels {
    public static class SeedData {
        public static void Initialize(IServiceProvider serviceProvider) {
            using var context = new Task9Context(
                    serviceProvider.GetRequiredService<DbContextOptions<Task9Context>>());

            if (context.Course.Any()) {
                return;   // DB has been seeded
            }
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Customers ON;");

            context.Course.AddRange(SeedCourses());

            context.Group.AddRange(SeedGroups());
            
            context.Student.AddRange(SeedStudents());
            context.SaveChanges();
        }

        //TODO Add More Data to Seed
        private static IEnumerable<Course> SeedCourses() {
            return new List<Course> {
                new() {
                    Name = "Course1",
                    Description = "SomeDescription"
                },
                new() {
                    Name = "Course2",
                    Description = "SomeDescription"
                }
            };
        }

        private static IEnumerable<Group> SeedGroups() {
            return new List<Group> {
                new() {
                    Name = "FirstGroup",
                    CourseId = 1
                }
            };
        }

        private static IEnumerable<Student> SeedStudents() {
            return new List<Student> {
                new() {
                    FirstName = "SomeName",
                    LastName = "SomeLastName",
                    GroupId = 1
                }
            };
        }
    }
}
