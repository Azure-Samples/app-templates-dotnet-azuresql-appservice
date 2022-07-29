using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public IndexModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        public Models.APIViewModels.InstructorResult Instructor { get; set; }
        
        public async Task OnGetAsync(int? id, int? courseID)
        {
            var response = await client.CreateClient("client").GetStringAsync("api/Instructors");
            Instructor = JsonConvert.DeserializeObject<Models.APIViewModels.InstructorResult>(response);
        }
    }
}