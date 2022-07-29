using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public DetailsModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        public Models.APIViewModels.Instructor Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.CreateClient("client").GetStringAsync("api/Instructors/" + id);
            Instructor = JsonConvert.DeserializeObject<Models.APIViewModels.Instructor>(response);

            if (Instructor == null)
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}
