using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public DetailsModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        public Models.APIViewModels.Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.CreateClient("client").GetStringAsync("api/Courses/" + id);
            Course = JsonConvert.DeserializeObject<Models.APIViewModels.Course>(response);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}