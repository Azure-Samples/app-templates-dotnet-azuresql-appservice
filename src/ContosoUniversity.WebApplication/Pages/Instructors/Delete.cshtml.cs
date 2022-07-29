using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Instructors
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public DeleteModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.CreateClient("client").DeleteAsync("api/Instructors/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return RedirectToAction("./Delete", new { id, saveChangesError = true });
        }
    }
}
