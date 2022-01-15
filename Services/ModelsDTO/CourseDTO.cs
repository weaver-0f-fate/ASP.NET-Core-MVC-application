using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.ModelsDTO {
    public class CourseDto : AbstractDto {
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
