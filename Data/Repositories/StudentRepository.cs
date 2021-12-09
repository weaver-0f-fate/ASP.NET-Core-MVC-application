using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class StudentRepository : AbstractRepository<Student> {
        public StudentRepository(Task9Context context, DbSet<Student> repo) : base(context, repo) { }

        public override async Task<IEnumerable<Student>> GetEntityListAsync() {
            return await Context.Students
                .Include(x => x.Group)
                .AsNoTracking()
                .ToListAsync();
        }

        protected override async Task<Student> GetItem(int id) {
            return await Context.Students
                .Include(x => x.Group)
                .Include(x => x.Group.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
