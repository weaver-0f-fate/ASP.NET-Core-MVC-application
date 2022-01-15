using System.ComponentModel.DataAnnotations;
using Core.Models;

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
        public int CourseId { get; set; }
        public string CourseName { get; set; }


        public StudentDto() { }

        public StudentDto(int? courseId, int? groupId) {
            if (courseId > 0) {
                CourseId = (int)courseId;
            }

            if (groupId > 0) {
                GroupId = (int)groupId;
            }
        }
    }
}
