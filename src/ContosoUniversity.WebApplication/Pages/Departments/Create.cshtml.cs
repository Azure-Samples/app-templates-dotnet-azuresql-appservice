using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public CreateModel(IHttpClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = await client.CreateClient("client").GetStringAsync("api/Instructors");
            var i = JsonConvert.DeserializeObject<Models.APIViewModels.InstructorResult>(response);
            ViewData["InstructorID"] = new SelectList(i.Instructors, "ID", "FullName");
            return Page();
        }

        [BindProperty]
        public Models.APIViewModels.Department Department { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await client.CreateClient("client").PostAsync("api/Departments", new StringContent(JsonConvert.SerializeObject(Department), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return Redirect("/Error");
        }
    }
}