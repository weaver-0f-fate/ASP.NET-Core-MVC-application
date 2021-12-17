using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class GroupService : AbstractService<Group, GroupDto> {
        public GroupService(GroupRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task DeleteAsync(int id) {
            var group = await Repository.GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == id)) {
                throw new ForeignEntitiesException();
            }
            await Repository.DeleteAsync(id);
        }
    }
}