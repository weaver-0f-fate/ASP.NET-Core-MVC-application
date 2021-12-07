#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class StudentsViewModel {
        public List<StudentDTO>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentGroup { get; set; }
    }
}
