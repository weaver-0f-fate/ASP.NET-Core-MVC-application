using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDto> {

        public StudentService(IRepository<Student> repository, IMapper mapper) : base(repository, mapper) { }
        protected override async Task<List<Student>> GetFilteredItemsAsync(FilteringParameters parameters = null) {
            var students = await Repository.GetEntityListAsync();

            if (parameters is null) {
                return students.ToList();
            }

            if (parameters.CourseFilter > 0) {
                students = students.Where(x => x.Group.CourseId == parameters.CourseFilter);
            }

            if (parameters.GroupFilter > 0) {
                students = students.Where(x => x.Group.Id == parameters.GroupFilter);
            }

            if (!string.IsNullOrEmpty(parameters.SearchString)) {
                students = students.Where( 
                    x => x.FirstName.Contains(parameters.SearchString)
                         || x.LastName.Contains(parameters.SearchString)
                         || x.Group.GroupName.Contains(parameters.SearchString));
            }
            return students.ToList();
        }
    }
}