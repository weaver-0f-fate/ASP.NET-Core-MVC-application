﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class GroupRepository : AbstractRepository<Group> {
        public GroupRepository(Task9Context context) : base(context, context.Groups) { }

        public override async Task<IEnumerable<Group>> GetEntityListAsync(IFilter<Group> filter) {
            var filteringExpression = filter.GetFilteringExpression();
            return await Context.Groups
                .Include(x => x.Course)
                .Include(x => x.Students)
                .Where(filteringExpression)
                .AsNoTracking()
                .ToListAsync();
        }

        protected override async Task<Group> GetItemAsync(int id) {
            return await Context.Groups
                .Include(x => x.Course)
                .Include(x => x.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCourseIdByGroupId(int id) {
            var group = await Context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return group.CourseId;
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
