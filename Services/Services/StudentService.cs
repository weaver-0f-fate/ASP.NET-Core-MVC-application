using AutoMapper;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDto> {

        public StudentService(IRepository<Student> repository, IMapper mapper) : base(repository, mapper) { }
    }
}