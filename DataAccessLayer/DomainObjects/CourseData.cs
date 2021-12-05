using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DomainLayer.Models.TaskModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DomainObjects {
    public class CourseData {
        private readonly Task9Context _context;

        private CourseData(Task9Context context) {
            _context = context;
        }

        public static CourseData GetCourseData(Task9Context context) {
            return context is null ? null : new CourseData(context);
        }


        public async Task<List<Course>> GetCourses(string searchString = null) {
            var courses = from c in _context.Course select c;
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    s => s.CourseName!.Contains(searchString) 
                 || s.CourseDescription!.Contains(searchString));
            }

            return await courses.ToListAsync();
        }

        public async Task<Course> GetCourseById(int? id) {
            if (id is null) {
                return null;
            }
            var course = await _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return course;
        }

        public async Task CreateCourse(Course course) {
            _context.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCourse(Course course) {
            if (course is null) {
                return false;
            }
            try {
                _context.Update(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) {
                if (!CourseExists(course.Id)) {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteCourse(int id) {
            var course = await GetCourseById(id);
            if (_context.Group.Any(x => x.CourseId == course.Id)) {
                return false;
            }
            try {
                _context.Course.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private bool CourseExists(int id) {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
