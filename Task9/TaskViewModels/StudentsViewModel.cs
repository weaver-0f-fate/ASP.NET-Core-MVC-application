#nullable enable
using System.Collections.Generic;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class StudentsViewModel {
        public List<StudentDTO>? FilteredStudents { get; set; }
        public int? SelectedGroupId { get; set; }

    }
}
