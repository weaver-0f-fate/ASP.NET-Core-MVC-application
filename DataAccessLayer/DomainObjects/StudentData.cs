using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DomainLayer.Models.TaskModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DomainObjects {
    public class StudentData {
        private readonly Task9Context _context;

        private StudentData(Task9Context context) {
            _context = context;
        }

        public static StudentData GetStudentData(Task9Context context) {
            return context is null ? null : new StudentData(context);
        }

        public async Task<List<Student>> GetStudents(string studentGroup, string searchString) {
            var students = GetStudentsWithGroups();
            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName!.Contains(searchString)
                 || x.LastName!.Contains(searchString)
                 || x.Group.GroupName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(studentGroup)) {
                students = students.Where(x => x.Group.GroupName == studentGroup);
            }

            return await students.ToListAsync();
        }

        public IEnumerable<Group> GetQueryableGroups() {
            var groups = from x in _context.Group orderby x.GroupName select x;
            return groups.AsNoTracking();
        }

        public async Task<List<string>> GetGroupsList() {
            var groups = from m in _context.Student orderby m.GroupId select m.Group.GroupName;
            return await groups.AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<Student> GetStudentById(int? id) {
            if (id is null) {
                return null;
            }
            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student is null) {
                return null;
            }

            student.Group = _context.Group.FirstOrDefault(x => x.Id == student.GroupId);
            student.Group.Course = _context.Course.FirstOrDefault(x => x.Id == student.Group.CourseId);
            return student;
        }

        public async Task CreateStudent(Student student) {
            _context.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateStudent(Student student) {
            if (student is null) {
                return false;
            }
            try {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) {
                if (!StudentExists(student.Id)) {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteStudent(int id) {
            var student = await GetStudentById(id);
            try {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private bool StudentExists(int id) {
            return _context.Student.Any(e => e.Id == id);
        }

        private IQueryable<Student> GetStudentsWithGroups() {
            var students = from s in _context.Student select s;
            foreach (var student in students) {
                student.Group = _context.Group.FirstOrDefault(x => x.Id == student.GroupId);
            }
            return students;
        }
    }
}
