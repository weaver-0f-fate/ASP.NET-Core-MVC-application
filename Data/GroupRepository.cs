using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class GroupRepository : IRepository<Group> {
        private readonly Task9Context _context;
        private bool _disposed;

        private GroupRepository(Task9Context context) {
            _context = context;
        }

        public static GroupRepository GetGroupRepository(Task9Context context) {
            return context is null ? null : new GroupRepository(context);
        }

        public IEnumerable<Group> GetEntityList() {
            var groups = GetGroupsWithCourse();
            return groups.ToListAsync().Result;
        }

        public IEnumerable<Group> GetEntityList(string groupCourse, string searchString) {
            var groups = GetEntityList();
            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(
                    x => x.GroupName!.Contains(searchString)
                 || x.Course.CourseName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(groupCourse)) {
                groups = groups.Where(x => x.Course.CourseName == groupCourse);
            }

            return groups;
        }

        public IEnumerable<Course> GetQueryableCourses() {
            var courses = from x in _context.Course orderby x.CourseName select x;
            return courses.AsNoTracking();
        }

        public IEnumerable<string> GetCoursesList() {
            var courses = from m in _context.Group orderby m.CourseId select m.Course.CourseName;
            return courses.AsNoTracking().Distinct().ToListAsync().Result;
        }

        public Group GetEntity(int id) {
            if (id < 0) {
                return null;
            }
            var group = _context.Group
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id)
                .Result;
            if (group is null) {
                return null;
            }

            group.Course = _context.Course.FirstOrDefault(x => x.Id == group.CourseId);
            return group;
        }

        public void Create(Group group) {
            _context.Add(group);
            Save();
        }

        public void Update(Group group) {
            if (group is null) {
                return;
            }
            _context.Update(group);
            Save();
        }

        public void Delete(int id) {
            var group = GetEntity(id);
            if (_context.Student.Any(x => x.GroupId == group.Id)) {
                throw new Exception();
            }
            _context.Group.Remove(group);
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

        public bool GroupExists(int id) {
            return _context.Group.Any(e => e.Id == id);
        }

        private IQueryable<Group> GetGroupsWithCourse() {

            var groups = from g in _context.Group
                         from c in _context.Course
                         where g.CourseId == c.Id
                         select new Group {
                             Id = g.Id,
                             GroupName = g.GroupName,
                             CourseId = g.CourseId,
                             Course = c
                         };
            return groups;
        }
    }
}
