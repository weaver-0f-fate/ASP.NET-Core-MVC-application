using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class GroupRepository : AbstractRepository<Group> {
        public GroupRepository(Task9Context context) : base(context) { }

        public override async Task<IEnumerable<Group>> GetEntityListAsync() {
            return await _context.Groups.Include(x => x.Course)
                .Include(x => x.Students).AsNoTracking().ToListAsync();
        }

        public override async Task<Group> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }

            var group = await _context.Groups
                .Include(x => x.Course)
                .Include(x => x.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (group is null) {
                throw new NoEntityException();
            }
            return group;
        }

        public override async Task DeleteAsync(int id) {
            var group = await GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == group.Id)) {
                throw new NoEntityException();
            }
            _context.Groups.Remove(group);
            await SaveAsync();
        }

        public override async Task<bool> Exists(int id) {
            return await _context.Groups.AnyAsync(e => e.Id == id);
        }

        
    }
}
