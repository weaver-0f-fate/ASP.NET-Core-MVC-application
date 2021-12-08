using System.ComponentModel.DataAnnotations;
using ServicesInterfaces;

namespace Services.ModelsDTO {
    public class StudentDTO : IDTO {

        public int Id { get; set; }

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
