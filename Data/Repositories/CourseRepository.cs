using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class CourseRepository : AbstractRepository<Course> {
        private CourseRepository(Task9Context context) : base(context) { }
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
            return course;
        }

        public override async Task DeleteAsync(int id) {
            var course = GetEntityAsync(id);
            _context.Courses.Remove(course.Result);
            await SaveAsync();
        }

        public bool CourseExists(int id) {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
