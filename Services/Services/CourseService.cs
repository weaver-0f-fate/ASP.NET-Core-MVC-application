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
        protected override async Task<List<Course>> GetFilteredItemsAsync(FilteringParameters parameters = null) {
            var courses = await Repository.GetEntityListAsync();

            if (parameters is null) {
                return courses.ToList();
            }

            if (!string.IsNullOrEmpty(parameters.SearchString)) {
                courses = courses.Where(x => x.CourseName.Contains(parameters.SearchString)
                                    || x.CourseDescription.Contains(parameters.SearchString));
            }
            return courses.ToList();
        }
    }
}