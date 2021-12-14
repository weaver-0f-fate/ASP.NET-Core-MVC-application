using AutoMapper;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class CourseService : AbstractService<Course, CourseDto> {
        public CourseService(IRepository<Course> repository, IMapper mapper) : base(repository, mapper){ }
    }
}