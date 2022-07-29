using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public DeleteModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        [BindProperty]
        public Models.APIViewModels.Department Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //teste 02
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.CreateClient("client").GetStringAsync("api/Departments/" + id);
            Department = JsonConvert.DeserializeObject<Models.APIViewModels.Department>(response);

            if (Department == null)
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

            var response = await client.CreateClient("client").DeleteAsync("api/Departments/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return RedirectToAction("./Delete", new { id, saveChangesError = true });
        }
    }
}