namespace Core.Models {
    public class Student : AbstractModel {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
