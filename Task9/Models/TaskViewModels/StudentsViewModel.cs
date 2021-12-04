#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.Models.TaskModels;

namespace Task9.Models.TaskViewModels {
    public class StudentsViewModel {
        public List<Student>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentGroup { get; set; }
    }
}
