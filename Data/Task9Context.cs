using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class Task9Context : DbContext
    {
        public Task9Context (DbContextOptions<Task9Context> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}
