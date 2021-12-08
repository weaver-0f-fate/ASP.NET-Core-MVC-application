#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class GroupViewModel {
        public List<GroupDTO>? FilteredGroups { get; set; }
        public SelectList? CoursesInSelectList { get; set; }
        public string? SelectedCourse { get; set; }
    }
}
