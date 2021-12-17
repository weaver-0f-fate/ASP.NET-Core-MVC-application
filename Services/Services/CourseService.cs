using AutoMapper;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class CourseService : AbstractService<Course, CourseDto> {
        public CourseService(CourseRepository repository, IMapper mapper) : base(repository, mapper){ }
    }
}