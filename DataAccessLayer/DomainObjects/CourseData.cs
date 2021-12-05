using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DomainLayer.Models.TaskModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DomainObjects {
    public static class CourseData {

        public static IQueryable<Course> GetCourses(Task9Context context, string searchString = null) {
            if (context is null) {
                return null;
            }
            var courses = from c in context.Course select c;
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    s => s.CourseName!.Contains(searchString) 
                 || s.CourseDescription!.Contains(searchString));
            }

            return courses;
        }

        public static async Task<Course> GetCourseById(Task9Context context, int? id) {
            if (id is null || context is null) {
                return null;
            }
            var course = await context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return course;
        }

        public static async Task CreateCourse(Task9Context context, Course course) {
            context.Add(course);
            await context.SaveChangesAsync();
        }

        public static async Task<bool> UpdateCourse(Task9Context context, Course course) {
            try {
                context.Update(course);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) {
                if (!CourseExists(context, course.Id)) {
                    return false;
                }
                throw;
            }
        }

        public static async Task<bool> DeleteCourse(Task9Context context, int id) {
            var course = await GetCourseById(context, id);
            if (context.Group.Any(x => x.CourseId == course.Id)) {
                return false;
            }
            try {
                context.Course.Remove(course);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private static bool CourseExists(Task9Context context, int id) {
            return context.Course.Any(e => e.Id == id);
        }
    }
}
