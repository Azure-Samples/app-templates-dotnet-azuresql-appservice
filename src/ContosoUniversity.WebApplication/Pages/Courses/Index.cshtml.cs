using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public IndexModel(IHttpClientFactory client)
        { 
            this.client = client;
        }

        public Models.APIViewModels.CoursesResult Course { get; set; }

        public async Task OnGetAsync()
        {
            var response = await client.CreateClient("client").GetStringAsync("api/Courses");
            Course = JsonConvert.DeserializeObject<Models.APIViewModels.CoursesResult>(response);
        }
    }
}