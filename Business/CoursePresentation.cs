using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Data;
using Services.ModelsDTO;
using ServicesInterfaces;

namespace Services {
    public class CoursePresentation : IPresentationItem<CourseDTO> {
        private readonly CourseRepository _courseRepository;
        private readonly GroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public CoursePresentation(Task9Context context, IMapper mapper) {
            _courseRepository = CourseRepository.GetCourseRepository(context);
            _groupRepository = GroupRepository.GetGroupRepository(context);
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllItemsAsync(string searchString) {
            var courses = await _courseRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    x => x.CourseName.Contains(searchString) 
                         || x.CourseDescription.Contains(searchString));
            }

            var coursesDTOs = courses.Select(x => _mapper.Map<CourseDTO>(x));
            return coursesDTOs;
        }

        public async Task<CourseDTO> GetItemAsync(int? id) {
            if (id is null) {
                throw new Exception();
            }
            var course = await _courseRepository.GetEntityAsync((int)id);
            if (course == null) {
                throw new Exception();
            }

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task CreateItemAsync(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                    CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.CreateAsync(course);
        }

        public async Task UpdateItemAsync(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.UpdateAsync(course);
        }
        public async Task DeleteItemAsync(int id) {
            var groups = await _groupRepository.GetEntityListAsync();
            if (groups.Any(x => x.CourseId == id)) {
                throw new Exception();
            }
            await _courseRepository.DeleteAsync(id);
        }

        public bool ItemExists(int id) {
            return _courseRepository.CourseExists(id);
        }
    }
}