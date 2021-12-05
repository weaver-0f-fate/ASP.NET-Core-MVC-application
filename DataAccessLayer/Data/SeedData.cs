using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Models.TaskModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Data {
    public static class SeedData {
        public static void Initialize(IServiceProvider serviceProvider) {
            using var context = new Data.Task9Context(
                    serviceProvider.GetRequiredService<DbContextOptions<Task9Context>>());

            if (context.Course.Any()) {
                return;   // DB has been seeded
            }

            context.Course.AddRange(SeedCourses());

            context.Group.AddRange(SeedGroups());
            
            context.Student.AddRange(SeedStudents());
            context.SaveChanges();
        }

        private static IEnumerable<Course> SeedCourses() {
            return new List<Course> {
                new() {
                    CourseName = "Linear Algebra",
                    CourseDescription = "Math Linear Algebra Course."
                },
                new() {
                    CourseName = "C# Programming",
                    CourseDescription = "C# programming course."
                },
                new() {
                    CourseName = "Hardware",
                    CourseDescription = "Studying Hardware Course."
                }
            };
        }

        private static IEnumerable<Group> SeedGroups() {
            return new List<Group> {
                new() {
                    GroupName = "Linear Algebra First Group",
                    CourseId = 1
                },
                new() {
                    GroupName = "Linear Algebra Second Group",
                    CourseId = 1
                },
                new() {
                    GroupName = "C# Programming First Group",
                    CourseId = 2
                },
                new() {
                    GroupName = "C# Programmin Second Group",
                    CourseId = 2
                },
                new() {
                    GroupName = "Hardware First Group",
                    CourseId = 3
                },
                new() {
                    GroupName = "Hardware Second Group",
                    CourseId = 3
                }
            };
        }

        private static IEnumerable<Student> SeedStudents() {
            return new List<Student> {
                new() {
                    FirstName = "Isabell",
                    LastName = "Farrington",
                    GroupId = 1
                },
                new() {
                    FirstName = "Habib",
                    LastName = "Easton",
                    GroupId = 1
                },
                new() {
                    FirstName = "Damian",
                    LastName = "Fraser",
                    GroupId = 1
                },
                new() {
                    FirstName = "Sasha",
                    LastName = "Phillips",
                    GroupId = 1
                },
                new() {
                    FirstName = "Gene",
                    LastName = "Dyer",
                    GroupId = 1
                },
                new() {
                    FirstName = "Abu",
                    LastName = "Colon",
                    GroupId = 1
                },
                new() {
                    FirstName = "Nikhil",
                    LastName = "Barlow",
                    GroupId = 1
                },
                new() {
                    FirstName = "Jodi",
                    LastName = "Anthony",
                    GroupId = 1
                },
                new() {
                    FirstName = "Grant",
                    LastName = "Warner",
                    GroupId = 1
                },
                new() {
                    FirstName = "Maggie",
                    LastName = "Santos",
                    GroupId = 1
                },
                new() {
                    FirstName = "Darius",
                    LastName = "Mccartney",
                    GroupId = 1
                },
                new() {
                    FirstName = "Bethany",
                    LastName = "Conrad",
                    GroupId = 2
                },
                new() {
                    FirstName = "Konnor",
                    LastName = "Valencia",
                    GroupId = 2
                },
                new() {
                    FirstName = "Kaiden",
                    LastName = "Greenwood",
                    GroupId = 2
                },
                new() {
                    FirstName = "Aniqa",
                    LastName = "Rooney",
                    GroupId = 2
                },
                new() {
                    FirstName = "Awais",
                    LastName = "Deleon",
                    GroupId = 2
                },
                new() {
                    FirstName = "Jude",
                    LastName = "Ryder",
                    GroupId = 3
                },
                new() {
                    FirstName = "Anushka",
                    LastName = "Gomez",
                    GroupId = 3
                },
                new() {
                    FirstName = "Jayde",
                    LastName = "Acevedo",
                    GroupId = 3
                },
                new() {
                    FirstName = "Jacqueline",
                    LastName = "Driscoll",
                    GroupId = 4
                },
                new() {
                    FirstName = "Lochlan",
                    LastName = "Acevedo",
                    GroupId = 4
                },
                new() {
                    FirstName = "Jacqueline",
                    LastName = "Ellwood",
                    GroupId = 5
                },
                new() {
                    FirstName = "Greta",
                    LastName = "Wang",
                    GroupId = 5
                },
                new() {
                    FirstName = "Anushka",
                    LastName = "Silva",
                    GroupId = 5
                },
                new() {
                    FirstName = "Jude",
                    LastName = "Goodwin",
                    GroupId = 5
                },
                new() {
                    FirstName = "Allen",
                    LastName = "Carver",
                    GroupId = 5
                },
                new() {
                    FirstName = "Florrie",
                    LastName = "Wills",
                    GroupId = 6
                },
                new() {
                    FirstName = "Marek",
                    LastName = "Brady",
                    GroupId = 6
                },
            };
        }
    }
}
