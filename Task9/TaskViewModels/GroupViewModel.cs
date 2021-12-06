#nullable enable
using System.Collections.Generic;
using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task9.TaskViewModels {
    public class GroupViewModel {
        public List<Group>? Groups { get; set; }
        public SelectList? Courses { get; set; }
        public string? GroupCourse { get; set; }
    }
}
