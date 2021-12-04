using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models.TaskModels {
    public class Course {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Group> Groups { get; set; }
    }
}
