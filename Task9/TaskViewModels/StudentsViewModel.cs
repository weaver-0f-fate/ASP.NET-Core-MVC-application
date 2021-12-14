#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class StudentsViewModel {
        public List<StudentDto>? FilteredStudents { get; set; }

        public SelectList Courses { get; set; }
        public CourseDto? SelectedCourseDto { get; set; }

        public SelectList Groups { get; set; }
        public GroupDto? SelectedGroupDto { get; set; }

        public StudentsViewModel(SelectList courses, SelectList groups) {
            Courses = courses;
            Groups = groups;
            FilteredStudents = new List<StudentDto>();
            SelectedCourseDto = new CourseDto();
            SelectedGroupDto = new GroupDto();
        }
    }
}
