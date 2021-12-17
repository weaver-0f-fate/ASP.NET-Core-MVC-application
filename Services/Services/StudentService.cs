using AutoMapper;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDto> {

        public StudentService(StudentRepository repository, IMapper mapper) : base(repository, mapper) { }
    }
}