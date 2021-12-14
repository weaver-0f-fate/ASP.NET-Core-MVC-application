using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class CourseRepository : AbstractRepository<Course> {
        public CourseRepository(Task9Context context) : base(context, context.Courses) { }

        public override async Task<IEnumerable<Course>> GetEntityListAsync() {
            return await Context.Courses
                .Include(x => x.Groups)
                .AsNoTracking()
                .ToListAsync();
        }

        protected override async Task<Course> GetItemAsync(int id) {
            return await Context.Courses
                .Include(x => x.Groups)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
