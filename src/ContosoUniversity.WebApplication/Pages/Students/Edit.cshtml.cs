using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> logger;
        private readonly IHttpClientFactory client;

        public EditModel(ILogger<EditModel> logger, IHttpClientFactory client)
        {
            this.logger = logger;
            this.client = client;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
    
            var response = await client.CreateClient("client").PutAsync("api/Students/" + Student.Id, new StringContent(JsonConvert.SerializeObject(Student), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return Redirect("/Error");
        }
    }
}