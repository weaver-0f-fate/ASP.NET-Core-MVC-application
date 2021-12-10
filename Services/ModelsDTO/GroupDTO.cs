using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ServicesInterfaces;

namespace Services.ModelsDTO {
    public class GroupDTO : IDTO {
        public int Id { get; set; }
        [StringLength(30)]
        [DisplayName("Group Name")]
        [Required]
        public string GroupName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
