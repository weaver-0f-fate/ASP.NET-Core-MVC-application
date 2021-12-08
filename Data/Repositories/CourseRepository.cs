using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class CourseRepository : AbstractRepository<Course> {
        public CourseRepository(Task9Context context) : base(context) { }
        public static CourseRepository GetCourseRepository(Task9Context context) {
            return context is null ? null : new CourseRepository(context);
        }

        public override async Task<IEnumerable<Course>> GetEntityListAsync() {
            return await _context.Courses.Include(x => x.Groups).AsNoTracking().ToListAsync();
        }

        public override async Task<Course> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var course = await _context.Courses
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
            _context.Courses.Remove(course);
            await SaveAsync();
        }

        public override async Task<bool> Exists(int id) {
            return await _context.Courses.AnyAsync(e => e.Id == id);
        }
    }
}
