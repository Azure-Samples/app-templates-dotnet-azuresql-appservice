using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> logger;
        private readonly IHttpClientFactory client;

        public DetailsModel(ILogger<DetailsModel> logger, IHttpClientFactory client)
        {
            this.logger = logger;
            this.client = client;
        }

        public Models.APIViewModels.Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.CreateClient("client").GetStringAsync("api/Students/" + id);
            Student = JsonConvert.DeserializeObject<Models.APIViewModels.Student>(response);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}