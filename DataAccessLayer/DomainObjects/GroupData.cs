using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DomainLayer.Models.TaskModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DomainObjects {
    public class GroupData {
        private readonly Task9Context _context;

        private GroupData(Task9Context context) {
            _context = context;
        }

        public static GroupData GetGroupData(Task9Context context) {
            return context is null ? null : new GroupData(context);
        }

        public async Task<List<Group>> GetGroups(string groupCourse, string searchString) {
            var groups = GetGroupsWithCourse();
            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(
                    x => x.GroupName!.Contains(searchString)
                 || x.Course.CourseName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(groupCourse)) {
                groups = groups.Where(x => x.Course.CourseName == groupCourse);
            }

            return await groups.ToListAsync();
        }

        public IEnumerable<Course> GetQueryableCourses() {
            var courses = from x in _context.Course orderby x.CourseName select x;
            return courses.AsNoTracking();
        }

        public async Task<List<string>> GetCoursesList() {
            var courses = from m in _context.Group orderby m.CourseId select m.Course.CourseName;
            return await courses.AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<Group> GetGroupById(int? id) {
            if (id is null) {
                return null;
            }
            var group = await _context.Group
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group is null) {
                return null;
            }

            group.Course = _context.Course.FirstOrDefault(x => x.Id == group.CourseId);
            return group;
        }

        public async Task CreateGroup(Group group) {
            _context.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateGroup(Group group) {
            if (group is null) {
                return false;
            }
            try {
                _context.Update(group);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) {
                if (!GroupExists(group.Id)) {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteGroup(int id) {
            var group = await GetGroupById(id);
            if (_context.Student.Any(x => x.GroupId == group.Id)) {
                return false;
            }
            try {
                _context.Group.Remove(group);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private bool GroupExists(int id) {
            return _context.Group.Any(e => e.Id == id);
        }

        private IQueryable<Group> GetGroupsWithCourse() {
            var groups = from g in _context.Group select g;
            foreach (var @group in groups) {
                group.Course = _context.Course.FirstOrDefault(s => s.Id == group.CourseId);
            }
            return groups;
        }

    }
}
