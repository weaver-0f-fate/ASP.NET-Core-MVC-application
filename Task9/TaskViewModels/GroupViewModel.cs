#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels.ModelsDTO;

namespace Task9.TaskViewModels {
    public class GroupViewModel {
        public List<GroupDTO>? Groups { get; set; }
        public SelectList? Courses { get; set; }
        public string? GroupCourse { get; set; }
    }
}
