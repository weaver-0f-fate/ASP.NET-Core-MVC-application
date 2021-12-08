using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Data.Repositories;
using Services.ModelsDTO;

namespace Services.Services {
    public class CourseService : AbstractService<Course, CourseDTO> {


        public CourseService(CourseRepository repository, IMapper mapper) : base(repository, mapper){ }


        public override async Task<IEnumerable<string>> GetNames() {
            var courses = await Repository.GetEntityListAsync();
            return courses.Select(x => x.CourseName);
        }

        protected override async Task<List<Course>> GetFilteredItems(string searchString = null, string filter = null) {
            var courses = await Repository.GetEntityListAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(x => x.CourseName.Contains(searchString)
                                    || x.CourseDescription.Contains(searchString));
            }
            return courses.ToList();
        }
    }
}