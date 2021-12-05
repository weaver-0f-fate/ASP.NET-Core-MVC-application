using Microsoft.EntityFrameworkCore;
using Task9.Models.TaskModels;

namespace Task9.Data
{
    public class Task9Context : DbContext
    {
        public Task9Context (DbContextOptions<Task9Context> options)
            : base(options)
        { }

        public DbSet<Course> Course { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbSet<Student> Student { get; set; }
    }
}
