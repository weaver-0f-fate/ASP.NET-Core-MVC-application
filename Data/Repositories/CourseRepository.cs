using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class CourseRepository : AbstractRepository<Course> {
        public CourseRepository(Task9Context context, DbSet<Course> repo) : base(context, repo) { }

        public override async Task<IEnumerable<Course>> GetEntityListAsync() {
            return await Context.Courses.Include(x => x.Groups).AsNoTracking().ToListAsync();
        }

        public override async Task<Course> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var course = await Context.Courses
                .Include(x => x.Groups)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course is null) {
                throw new NoEntityException();
            }
            return course;
        }

        public override async Task DeleteAsync(int id) {
            var course = await GetEntityAsync(id);
            Context.Courses.Remove(course);
            await SaveAsync();
        }
    }
}
