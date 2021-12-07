using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class StudentRepository : AbstractRepository<Student> {
        private StudentRepository(Task9Context context) : base(context) { }
        public static StudentRepository GetStudentData(Task9Context context) {
            return context is null ? null : new StudentRepository(context);
        }

        public override IEnumerable<Student> GetEntityList() {
            var students = GetStudentsWithGroups();
            return students.ToListAsync().Result;
        }

        public override IEnumerable<Student> GetEntityList(string searchString) {
            var students = GetEntityList();
            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName!.Contains(searchString)
                         || x.LastName!.Contains(searchString)
                         || x.Group.GroupName!.Contains(searchString));
            }
            return students;
        }

        public IEnumerable<Student> GetEntityList(string studentGroup, string searchString) {
            var students = GetEntityList(searchString);
            if (!string.IsNullOrEmpty(studentGroup)) {
                students = students.Where(x => x.Group.GroupName == studentGroup);
            }
            return students;
        }

        public IEnumerable<Group> GetQueryableGroups() {
            var groups = from x in _context.Group orderby x.GroupName select x;
            return groups.AsNoTracking();
        }

        public IEnumerable<string> GetGroupsList() {
            var groups = from m in _context.Student orderby m.GroupId select m.Group.GroupName;
            return groups.AsNoTracking().Distinct().ToListAsync().Result;
        }

        public override Student GetEntity(int id) {
            if (id < 0) {
                return null;
            }
            var student = _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id)
                .Result;
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

        public override void Delete(int id) {
            var student = GetEntity(id);
            _context.Student.Remove(student);
            Save();
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
