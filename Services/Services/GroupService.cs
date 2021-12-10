using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class GroupService : AbstractService<Group, GroupDTO> {

        public GroupService(GroupRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task DeleteAsync(int id) {
            var group = await Repository.GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == id)) {
                throw new ForeignEntitiesException();
            }
            await Repository.DeleteAsync(id);
        }

        protected override async Task<List<Group>> GetFilteredItems(string searchString = null, string filter = null) {
            var groups = await Repository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(filter)) {
                groups = groups.Where(x => x.Course.CourseName.Contains(filter));
            }

            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(x => x.GroupName.Contains(searchString)
                                             || x.Course.CourseName.Contains(searchString));
            }
            return groups.ToList();
        }
    }
}