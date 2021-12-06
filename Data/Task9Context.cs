using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class Task9Context : DbContext
    {
        public Task9Context (DbContextOptions<Task9Context> options) : base(options) { }

        public DbSet<Course> Course { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbSet<Student> Student { get; set; }
    }
}
