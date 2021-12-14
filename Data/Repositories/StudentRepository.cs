using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class StudentRepository : AbstractRepository<Student> {
        public StudentRepository(Task9Context context) : base(context, context.Students) { }

        public override async Task<IEnumerable<Student>> GetEntityListAsync() {
            return await Context.Students
                .Include(x => x.Group)
                .AsNoTracking()
                .ToListAsync();
        }

        protected override async Task<Student> GetItemAsync(int id) {
            return await Context.Students
                .Include(x => x.Group)
                .Include(x => x.Group.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
