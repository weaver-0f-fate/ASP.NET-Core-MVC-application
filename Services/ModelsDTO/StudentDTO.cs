using System.ComponentModel.DataAnnotations;

namespace Services.ModelsDTO {
    public class StudentDto : AbstractDto {
        [StringLength(50)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public string CourseName { get; set; }
    }
}
