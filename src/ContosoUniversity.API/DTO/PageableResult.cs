namespace ContosoUniversity.API.DTO
{
    public class PageableResult
    {
        public int Pages { get; set; }

        public int CurrentPage { get; set; }
        public int Count { get; set; }
    }
}
