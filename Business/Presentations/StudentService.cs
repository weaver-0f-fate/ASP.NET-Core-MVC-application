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
    public class StudentService : IService<StudentDTO> {
        private readonly StudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(Task9Context context, IMapper mapper) {
            _studentRepository = StudentRepository.GetStudentData(context);
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllItemsAsync(string searchString = null, string studentGroup = null) {
            var students = await _studentRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(studentGroup)) {
                students = students.Where(
                    x => x.Group.GroupName == studentGroup);
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

        public async Task<StudentDTO> GetAsync(int? id) {
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

        public async Task CreateAsync(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.CreateAsync(student);
        }

        public async Task UpdateAsync(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.UpdateAsync(student);
        }

        public async Task DeleteAsync(int id) {
            await _studentRepository.DeleteAsync(id);
        }

        public bool ItemExists(int id) {
            return _studentRepository.StudentExists(id);
        }
    }
}