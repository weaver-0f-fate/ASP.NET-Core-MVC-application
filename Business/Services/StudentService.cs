using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDTO> {

        public StudentService(StudentRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override async Task<IEnumerable<string>> GetNames() {
            var students = await Repository.GetEntityListAsync();
            return students.Select(x => $"{x.FirstName} {x.LastName}");
        }

        protected override async Task<List<Student>> GetFilteredItems(string searchString = null, string filter = null) {
            var students = await Repository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(filter)) {
                students = students.Where(x => x.Group.GroupName.Contains(filter));
            }

            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName.Contains(searchString)
                         || x.LastName.Contains(searchString)
                         || x.Group.GroupName.Contains(searchString));
            }
            return students.ToList();
        }
    }
}