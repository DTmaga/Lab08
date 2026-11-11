namespace SchoolAppCoreMVC.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public DateTime HireDate { get; set; }
    }
}
