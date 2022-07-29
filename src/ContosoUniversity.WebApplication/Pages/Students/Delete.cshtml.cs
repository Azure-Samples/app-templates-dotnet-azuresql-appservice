using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.WebApplication.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity.WebApplication.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> logger;
        private readonly IHttpClientFactory client;

        public DeleteModel(ILogger<DeleteModel> logger, IHttpClientFactory client)
        {
            this.logger = logger;
            this.client = client;
        }

        [BindProperty]
        public Models.APIViewModels.Student Student { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
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

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var response = await client.CreateClient("client").DeleteAsync("api/Students/" + id);

            if(response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return RedirectToAction("./Delete", new { id, saveChangesError = true });
        }
    }
}