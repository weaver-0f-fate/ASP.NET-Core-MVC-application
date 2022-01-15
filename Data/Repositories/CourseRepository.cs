using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class CourseRepository : AbstractRepository<Course> {
        public CourseRepository(Task9Context context) : base(context, context.Courses) { }

        public override async Task<IEnumerable<Course>> GetEntityListAsync(IFilter<Course> filter) {
            var filteringExpression = filter.GetFilteringExpression();
            return await Context.Courses
                    .Include(x => x.Groups)
                    .Where(filteringExpression)
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
