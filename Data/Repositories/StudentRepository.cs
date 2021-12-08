using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class StudentRepository : AbstractRepository<Student> {
        private StudentRepository(Task9Context context) : base(context) { }
        public static StudentRepository GetStudentData(Task9Context context) {
            return context is null ? null : new StudentRepository(context);
        }

        public override async Task<IEnumerable<Student>> GetEntityListAsync() {
            var students = GetStudentsWithGroups();
            return await students.AsNoTracking().ToListAsync();
        }

        public override async Task<Student> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student is null) {
                return null;
            }

            student.Group = _context.Group.FirstOrDefault(x => x.Id == student.GroupId);
            if (student.Group is null) {
                throw new NullReferenceException();
            }
            student.Group.Course = _context.Course.FirstOrDefault(x => x.Id == student.Group.CourseId);
            return student;
        }

        public override async Task DeleteAsync(int id) {
            var student = GetEntityAsync(id).Result;
            _context.Student.Remove(student);
            await SaveAsync();
        }

        public bool StudentExists(int id) {
            return _context.Student.Any(e => e.Id == id);
        }

        private IQueryable<Student> GetStudentsWithGroups() {
            var students = from s in _context.Student
                from g in _context.Group
                where s.GroupId == g.Id
                select new Student {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GroupId = s.GroupId,
                    Group = g
                };
            return students;
        }
    }
}
