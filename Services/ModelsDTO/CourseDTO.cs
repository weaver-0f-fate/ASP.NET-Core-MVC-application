using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ServicesInterfaces;

namespace Services.ModelsDTO {
    public class CourseDTO : IDTO {
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
