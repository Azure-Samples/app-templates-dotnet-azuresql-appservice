using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public EditModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        [BindProperty]
        public Models.APIViewModels.Department Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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

            var responseI = await client.CreateClient("client").GetStringAsync("api/Instructors");
            var i = JsonConvert.DeserializeObject<Models.APIViewModels.InstructorResult>(responseI);
            ViewData["InstructorID"] = new SelectList(i.Instructors, "ID", "FullName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await client.CreateClient("client").PutAsync("api/Departments/" + id, new StringContent(JsonConvert.SerializeObject(Department), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return Redirect("/Error");
        }
    }
}
