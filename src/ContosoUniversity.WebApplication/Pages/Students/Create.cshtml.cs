using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace ContosoUniversity.WebApplication.Pages.Students
{
    public class CreateModel : PageModel
    { 
        private readonly ILogger<CreateModel> logger;
        private readonly IHttpClientFactory client;

        public CreateModel(ILogger<CreateModel> logger, IHttpClientFactory client)
        {
            this.logger = logger;
            this.client = client;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.APIViewModels.Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await client.CreateClient("client").PostAsync("api/Students", new StringContent(JsonConvert.SerializeObject(Student), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");
            else
                return Redirect("/Error");
        }
    }
}