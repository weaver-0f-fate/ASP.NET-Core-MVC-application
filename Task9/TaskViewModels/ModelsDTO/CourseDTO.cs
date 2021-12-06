using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Task9.TaskViewModels.ModelsDTO {
    public class CourseDTO {
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
    }
}
