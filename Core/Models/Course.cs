using System.Collections.Generic;

namespace Core.Models {
    public class Course : AbstractModel {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public List<Group> Groups { get; set; }
    }
}
