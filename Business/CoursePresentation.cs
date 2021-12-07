using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Core.ModelsDTO;
using Data;
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

        public async Task<IEnumerable<CourseDTO>> GetAllItems(string searchString) {
            var courses = await _courseRepository.GetEntityList();

            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    x => x.CourseName.Contains(searchString) 
                         || x.CourseDescription.Contains(searchString));
            }

            var coursesDTOs = courses.Select(x => _mapper.Map<CourseDTO>(x));
            return coursesDTOs;
        }

        public async Task<CourseDTO> GetItem(int? id) {
            if (id is null) {
                throw new Exception();
            }
            var course = await _courseRepository.GetEntity((int)id);
            if (course == null) {
                throw new Exception();
            }

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task CreateItem(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                    CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.Create(course);
        }

        public async Task UpdateItem(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.Update(course);
        }
        public async Task DeleteItem(int id) {
            var groups = await _groupRepository.GetEntityList();
            if (groups.Any(x => x.CourseId == id)) {
                throw new Exception();
            }
            await _courseRepository.Delete(id);
        }

        public bool ItemExists(int id) {
            return _courseRepository.CourseExists(id);
        }
    }
}