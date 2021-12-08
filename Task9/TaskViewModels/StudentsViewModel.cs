#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class StudentsViewModel {
        public List<StudentDTO>? FilteredStudents { get; set; }
        public SelectList? GroupsInSelectList { get; set; }
        public string? SelectedGroup { get; set; }
    }
}
