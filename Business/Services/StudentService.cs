using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDTO> {
        private readonly StudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(StudentRepository repository, IMapper mapper) : base(repository, mapper) {
            _studentRepository = repository;
            _mapper = mapper;
        }

        public override async Task<IEnumerable<StudentDTO>> GetAllItemsAsync(string searchString = null, string groupFilter = null) {
            var students = await _studentRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(groupFilter)) {
                students = students.Where(
                    x => x.Group.GroupName == groupFilter);
            }

            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName.Contains(searchString)
                         || x.LastName.Contains(searchString)
                         || x.Group.GroupName.Contains(searchString));
            }


            var studentDTOs = students.Select(x => _mapper.Map<StudentDTO>(x)).ToList();
            foreach (var studentDTO in studentDTOs) {
                studentDTO.GroupName = students.FirstOrDefault(x => x.Id == studentDTO.Id).Group.GroupName;
            }
            return studentDTOs;
        }

        public override async Task<IEnumerable<string>> GetNames() {
            var students = await _studentRepository.GetEntityListAsync();
            return students.Select(x => $"{x.FirstName} {x.LastName}");
        }

        public override async Task<StudentDTO> GetAsync(int? id) {
            if (id is null) {
                throw new NoEntityException();
            }
            var student = await _studentRepository.GetEntityAsync((int)id);
            if (student is null) {
                throw new NoEntityException();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return studentDTO;
        }

        public override async Task CreateAsync(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.CreateAsync(student);
        }

        public override async Task UpdateAsync(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.UpdateAsync(student);
        }

        public override async Task DeleteAsync(int id) {
            await _studentRepository.DeleteAsync(id);
        }
    }
}