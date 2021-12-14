using System.Collections.Generic;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class CoursesViewModel {
        public List<CourseDto>? FilteredCourses { get; set; }
        public string SearchString { get; set; }
    }
}