using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class CourseService : AbstractService<Course, CourseDto> {
        public CourseService(IRepository<Course> repository, IMapper mapper) : base(repository, mapper){ }
        protected override async Task<List<Course>> GetFilteredItemsAsync(string searchString = null, int? filter = null) {
            var courses = await Repository.GetEntityListAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(x => x.CourseName.Contains(searchString)
                                    || x.CourseDescription.Contains(searchString));
            }
            return courses.ToList();
        }
    }
}