using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models {
    public class Course {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public List<Group> Groups { get; set; }
    }
}
