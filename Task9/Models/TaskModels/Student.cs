using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models.TaskModels {
    public class Student {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}
