#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.Models.TaskModels;

namespace Task9.Models.TaskViewModels {
    public class GroupViewModel {
        public List<Group>? Groups { get; set; }
        public SelectList? Courses { get; set; }
        public string? GroupCourse { get; set; }
    }
}
