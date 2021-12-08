using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class GroupRepository : AbstractRepository<Group> {
        public GroupRepository(Task9Context context, DbSet<Group> repo) : base(context, repo) { }

        public override async Task<IEnumerable<Group>> GetEntityListAsync() {
            return await Context.Groups.Include(x => x.Course)
                .Include(x => x.Students).AsNoTracking().ToListAsync();
        }

        public override async Task<Group> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }

            var group = await Context.Groups
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
            Context.Groups.Remove(group);
            await SaveAsync();
        }
    }
}
