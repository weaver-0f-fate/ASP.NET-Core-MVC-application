using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core {
    public class Group {

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [DisplayName("Group Name")]
        [Required]
        public string GroupName { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public List<Student> Students { get; set; }
    }
}
