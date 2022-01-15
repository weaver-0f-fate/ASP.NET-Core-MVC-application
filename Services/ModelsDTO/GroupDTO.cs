using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.ModelsDTO {
    public class GroupDto : AbstractDto {
        [StringLength(30)]
        [DisplayName("Group Name")]
        [Required]
        public string GroupName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
