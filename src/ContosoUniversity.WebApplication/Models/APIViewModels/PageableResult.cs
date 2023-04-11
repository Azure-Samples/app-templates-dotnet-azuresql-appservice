namespace ContosoUniversity.WebApplication.Models.APIViewModels
{
    public class PageableResult
    {
        public int Count { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}
