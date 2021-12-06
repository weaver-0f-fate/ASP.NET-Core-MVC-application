using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models {
    public class Course {

        [Key]
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
