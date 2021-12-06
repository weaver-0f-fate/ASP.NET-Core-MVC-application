using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class StudentRepository : IStudentRepository {
        private readonly Task9Context _context;
        private bool _disposed;

        private StudentRepository(Task9Context context) {
            _context = context;
        }

        public static StudentRepository GetStudentData(Task9Context context) {
            return context is null ? null : new StudentRepository(context);
        }

        public IEnumerable<Student> GetStudentList() {
            var students = GetStudentsWithGroups();
            return students.ToListAsync().Result;
        }

        public IEnumerable<Student> GetStudentList(string studentGroup, string searchString) {
            var students = GetStudentList();
            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName!.Contains(searchString)
                 || x.LastName!.Contains(searchString)
                 || x.Group.GroupName!.Contains(searchString));
            }

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

        public Student GetStudent(int id) {
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

        public void Create(Student student) {
            _context.Add(student);
            Save();
        }

        public void Update(Student student) {
            if (student is null) {
                return;
            }
            _context.Update(student);
            Save();
        }

        public void Delete(int id) {
            var student = GetStudent(id);
            _context.Student.Remove(student);
            Save();
        }

        public void Save() {
            _context.SaveChanges();
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

        public bool StudentExists(int id) {
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
