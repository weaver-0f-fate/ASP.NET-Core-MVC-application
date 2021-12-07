using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class CourseRepository : AbstractRepository<Course> {
        private CourseRepository(Task9Context context) : base(context) { }
        public static CourseRepository GetCourseRepository(Task9Context context) {
            return context is null ? null : new CourseRepository(context);
        }

        public override async Task<IEnumerable<Course>> GetEntityListAsync() {
            var courses = from c in _context.Course select c;
            return await courses.AsNoTracking().ToListAsync();
        }

        public override async Task<Course> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var course = await _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return course;
        }

        public override async Task DeleteAsync(int id) {
            var course = GetEntityAsync(id);
            if (_context.Group.Any(x => x.CourseId == course.Id)) {
                throw new Exception();
            }
            _context.Course.Remove(course.Result);
            await SaveAsync();
        }

        public bool CourseExists(int id) {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
