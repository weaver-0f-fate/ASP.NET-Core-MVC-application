using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class StudentRepository : AbstractRepository<Student> {
        public StudentRepository(Task9Context context, DbSet<Student> repo) : base(context, repo) { }

        public override async Task<IEnumerable<Student>> GetEntityListAsync() {
            return await Context.Students.Include(x => x.Group).AsNoTracking().ToListAsync();
        }

        public override async Task<Student> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var student = await Context.Students
                .Include(x => x.Group)
                .Include(x => x.Group.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student is null) {
                throw new NoEntityException();
            }
            return student;
        }

        public override async Task DeleteAsync(int id) {
            var student = await GetEntityAsync(id);
            Context.Students.Remove(student);
            await SaveAsync();
        }
    }
}
