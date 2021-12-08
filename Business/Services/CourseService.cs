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
    public class CourseService : AbstractService<Course, CourseDTO> {
        private readonly CourseRepository _courseRepository;
        private readonly IMapper _mapper;


        public CourseService(CourseRepository repository, IMapper mapper) : base(repository, mapper) {
            _courseRepository = repository;
            _mapper = mapper;
        }

        public override async Task<IEnumerable<CourseDTO>> GetAllItemsAsync(string searchString, string filter = null) {
            var courses = await _courseRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    x => x.CourseName.Contains(searchString) 
                         || x.CourseDescription.Contains(searchString));
            }

            var coursesDTOs = courses.Select(x => _mapper.Map<CourseDTO>(x));
            return coursesDTOs;
        }

        public override async Task<CourseDTO> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }
            var course = await _courseRepository.GetEntityAsync((int)id);
            if (course == null) {
                throw new NoEntityException();
            }

            return _mapper.Map<CourseDTO>(course);
        }

        public override async Task CreateAsync(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                    CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.CreateAsync(course);
        }

        public override async Task UpdateAsync(CourseDTO item) {
            var course = new Course {
                Id = item.Id,
                CourseDescription = item.CourseDescription,
                CourseName = item.CourseName,
            };
            await _courseRepository.UpdateAsync(course);
        }
        public override async Task DeleteAsync(int id) {
            await _courseRepository.DeleteAsync(id);
        }

        public override async Task<IEnumerable<string>> GetNames() {
            var courses = await GetCourses();
            return courses.Select(x => x.CourseName);
        }

        private async Task<IEnumerable<CourseDTO>> GetCourses() {
            var courses = await _courseRepository.GetEntityListAsync();
            return courses.Select(x => _mapper.Map<CourseDTO>(x));
        }
    }
}