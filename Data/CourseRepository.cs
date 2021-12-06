using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class CourseRepository : ICourseRepository {
        private readonly Task9Context _context;
        private bool _disposed;

        private CourseRepository(Task9Context context){
            _context = context;
        }

        public static CourseRepository GetCourseRepository(Task9Context context) {
            return context is null ? null : new CourseRepository(context);
        }


        public IEnumerable<Course> GetCourseList(string searchString) {
            var courses = from c in _context.Course select c;
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    s => s.CourseName!.Contains(searchString) 
                 || s.CourseDescription!.Contains(searchString));
            }

            return courses.ToListAsync().Result;
        }

        public IEnumerable<Course> GetCourseList() {
            var courses = from c in _context.Course select c;
            return courses.ToListAsync().Result;
        }

        public Course GetCourse(int id) {
            if (id < 0) {
                return null;
            }
            var course = _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return course.Result;
        }

        public void Create(Course course) {
            _context.Add(course);
            Save();
        }

        public void Update(Course course) {
            if (course is null) {
                return;
            }
            _context.Update(course);
            Save();
        }

        public void Delete(int id) {
            var course = GetCourse(id);
            if (_context.Group.Any(x => x.CourseId == course.Id)) {
                throw new Exception();
            }
            _context.Course.Remove(course);
            Save();
        }

        public void Save() {
            _context.SaveChanges();
        }

        public bool CourseExists(int id) {
            return _context.Course.Any(e => e.Id == id);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
