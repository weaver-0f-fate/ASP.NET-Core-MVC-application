using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class GroupService : AbstractService<Group, GroupDto> {

        public GroupService(IRepository<Group> repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task DeleteAsync(int id) {
            var group = await Repository.GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == id)) {
                throw new ForeignEntitiesException();
            }
            await Repository.DeleteAsync(id);
        }

        protected override async Task<List<Group>> GetFilteredItemsAsync(string searchString = null, int? filter = null) {
            var groups = await Repository.GetEntityListAsync();

            if (filter > 0) {
                groups = groups.Where(x => x.Course.Id == filter);
            }

            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(x => x.GroupName.Contains(searchString)
                                             || x.Course.CourseName.Contains(searchString));
            }
            return groups.ToList();
        }
    }
}