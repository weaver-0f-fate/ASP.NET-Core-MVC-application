using System.Collections.Generic;

namespace Core.Models {
    public class Group {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Student> Students { get; set; }
    }
}
