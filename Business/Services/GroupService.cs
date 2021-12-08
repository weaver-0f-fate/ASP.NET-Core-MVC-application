using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Data;
using Data.Repositories;
using Services.ModelsDTO;
using ServicesInterfaces;

namespace Services.Presentations {
    public class GroupService : IService<GroupDTO> {
        private readonly GroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(Task9Context context, IMapper mapper) {
            _groupRepository = GroupRepository.GetGroupRepository(context);
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDTO>> GetAllItemsAsync(string searchString = null, string courseFilter = null) {
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

        public async Task<GroupDTO> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }
            var group = await _groupRepository.GetEntityAsync((int)id);
            if (group is null) {
                throw new NoEntityException();
            }

            return _mapper.Map<GroupDTO>(group);
        }

        public async Task CreateAsync(GroupDTO item) {
            var group = new Group {
                Id = item.Id,
                GroupName = item.GroupName,
                CourseId = item.CourseId
            };
            await _groupRepository.CreateAsync(group);
        }

        public async Task UpdateAsync(GroupDTO item) {
            var group = new Group {
                Id = item.Id,
                CourseId = item.CourseId,
                GroupName = item.GroupName
            };
            await _groupRepository.UpdateAsync(group);
        }

        public async Task DeleteAsync(int id) {
            var group = await _groupRepository.GetEntityAsync(id);
            if (group.Students.Any(x => x.GroupId == id)) {
                throw new ForeignEntitiesException();
            }
            await _groupRepository.DeleteAsync(id);
        }

        public bool ItemExists(int id) {
            return _groupRepository.GroupExists(id);
        }

        public async Task<IEnumerable<string>> GetNames() {
            var groups = await GetGroups();
            return groups.Select(x => x.GroupName);
        }

        public async Task<IEnumerable<Group>> GetGroups() {
            return await _groupRepository.GetEntityListAsync();
        }
    }
}