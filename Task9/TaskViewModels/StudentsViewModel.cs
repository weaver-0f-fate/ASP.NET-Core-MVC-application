#nullable enable
using System.Collections.Generic;
using DomainLayer.Models.TaskModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task9.TaskViewModels {
    public class StudentsViewModel {
        public List<Student>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentGroup { get; set; }
    }
}
