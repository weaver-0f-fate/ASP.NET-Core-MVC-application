using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Core.ModelsDTO;
using Data;
using ServicesInterfaces;

namespace Business {
    public class StudentPresentation : IPresentationItem<StudentDTO> {
        private readonly StudentRepository _studentRepository;
        private readonly GroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public StudentPresentation(Task9Context context, IMapper mapper) {
            _studentRepository = StudentRepository.GetStudentData(context);
            _groupRepository = GroupRepository.GetGroupRepository(context);
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllItems(string searchString = null, string studentGroup = null) {
            var students = await _studentRepository.GetEntityList();

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

        public async Task<StudentDTO> GetItem(int? id) {
            if (id is null) {
                throw new Exception();
            }
            var student = await _studentRepository.GetEntity((int)id);
            if (student is null) {
                throw new Exception();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return studentDTO;
        }

        public async Task CreateItem(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.Create(student);
        }

        public async Task UpdateItem(StudentDTO item) {
            var student = new Student {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                GroupId = item.GroupId
            };
            await _studentRepository.Update(student);
        }

        public async Task DeleteItem(int id) {
            await _studentRepository.Delete(id);
        }

        public bool ItemExists(int id) {
            return _studentRepository.StudentExists(id);
        }

        public async Task<IEnumerable<Group>> GetGroups() {
            return await _groupRepository.GetEntityList();
        }
    }
}