using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Data.Repositories;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class GroupService : AbstractService<Group, GroupDTO> {
        private readonly GroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(GroupRepository repository, IMapper mapper) : base(repository, mapper) {
            _groupRepository = repository;
            _mapper = mapper;
        }

        public override async Task<IEnumerable<GroupDTO>> GetAllItemsAsync(string searchString = null, string courseFilter = null) {
            var groups = await _groupRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(courseFilter)) {
                groups = groups.Where(x => x.Course.CourseName.Contains(courseFilter));
            }

            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(
                    x => x.GroupName.Contains(searchString) 
                         || x.Course.CourseName.Contains(searchString));
            }

            return groups.Select(x => _mapper.Map<GroupDTO>(x)).ToList();
        }

        public override async Task<GroupDTO> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }
            var group = await _groupRepository.GetEntityAsync((int)id);
            if (group is null) {
                throw new NoEntityException();
            }

            return _mapper.Map<GroupDTO>(group);
        }

        public override async Task CreateAsync(GroupDTO item) {
            var group = new Group {
                Id = item.Id,
                GroupName = item.GroupName,
                CourseId = item.CourseId
            };
            await _groupRepository.CreateAsync(group);
        }

        public override async Task UpdateAsync(GroupDTO item) {
            var group = new Group {
                Id = item.Id,
                CourseId = item.CourseId,
                GroupName = item.GroupName
            };
            await _groupRepository.UpdateAsync(group);
        }

        public override async Task DeleteAsync(int id) {
            var group = await _groupRepository.GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == id)) {
                throw new ForeignEntitiesException();
            }
            await _groupRepository.DeleteAsync(id);
        }

        public override async Task<IEnumerable<string>> GetNames() {
            var groups = await GetGroups();
            return groups.Select(x => x.GroupName);
        }

        private async Task<IEnumerable<Group>> GetGroups() {
            return await _groupRepository.GetEntityListAsync();
        }
    }
}