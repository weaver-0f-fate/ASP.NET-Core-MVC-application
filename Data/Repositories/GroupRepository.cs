using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class GroupRepository : AbstractRepository<Group> {
        private GroupRepository(Task9Context context) : base(context) { }
        public static GroupRepository GetGroupRepository(Task9Context context) {
            return context is null ? null : new GroupRepository(context);
        }

        public override async Task<IEnumerable<Group>> GetEntityListAsync() {
            var groups = GetGroupsWithCourse();
            return await groups.AsNoTracking().ToListAsync();
        }

        public override async Task<Group> GetEntityAsync(int id) {
            if (id < 0) {
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

        public override async Task DeleteAsync(int id) {
            var group = GetEntityAsync(id).Result;
            if (_context.Student.Any(x => x.GroupId == group.Id)) {
                throw new NoEntityException();
            }
            _context.Group.Remove(group);
            await SaveAsync();
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
