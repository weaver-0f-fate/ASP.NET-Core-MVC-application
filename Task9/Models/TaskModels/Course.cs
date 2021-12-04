using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models.TaskModels {
    public class Course {

        public int Id { get; set; }

        [StringLength(30)]
        [DisplayName("Course Name")]
        [Required]
        public string CourseName { get; set; }

        [StringLength(400)]
        [DisplayName("Course Description")]
        [Required]
        public string CourseDescription { get; set; }

        public List<Group> Groups { get; set; }
    }
}
