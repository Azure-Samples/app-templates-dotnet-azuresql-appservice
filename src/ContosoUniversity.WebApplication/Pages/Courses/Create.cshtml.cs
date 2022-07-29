using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.WebApplication.Pages.Courses
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
            var response = await client.CreateClient("client").GetStringAsync("api/Departments");
            var dep = JsonConvert.DeserializeObject<Models.APIViewModels.DepartmentResult>(response);
            ViewData["DepartmentID"] = new SelectList(dep.Departments, "ID", "Name");

            return Page();
        }

        [BindProperty]
        public Models.APIViewModels.Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var response = await client.CreateClient("client").PostAsync("api/Courses", new StringContent(JsonConvert.SerializeObject(Course), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return RedirectToPage("/Error");
        }
    }
}