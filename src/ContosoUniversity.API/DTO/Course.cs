namespace ContosoUniversity.API.DTO
{
    public class Course
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Department Department { get; set; }
    }
}
