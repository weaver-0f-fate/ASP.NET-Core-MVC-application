using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models.TaskModels {
    public class Course {

        public int Id { get; set; }

        [StringLength(30)]
        [Required]
        public string Name { get; set; }

        [StringLength(400)]
        [Required]
        public string Description { get; set; }

        public List<Group> Groups { get; set; }
    }
}
