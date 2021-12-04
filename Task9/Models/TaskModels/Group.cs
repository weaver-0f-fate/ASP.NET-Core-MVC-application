using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models.TaskModels {
    public class Group {

        public int Id { get; set; }
        public string Name { get; set; }



        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public List<Student> Students { get; set; }
    }
}
