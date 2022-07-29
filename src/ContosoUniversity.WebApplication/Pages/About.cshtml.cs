using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity.WebApplication.Pages
{
    public class AboutModel : PageModel
    {
        private readonly IHttpClientFactory client;
        private readonly ILogger<AboutModel> logger;

        public string Msg { get; set; }

        public AboutModel(IHttpClientFactory client, ILogger<AboutModel> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpClient cli = client.CreateClient("client");
            Msg = "BaseAddress = " + cli.BaseAddress + " ---- ";
            try
            {
                
                logger.LogInformation("Base Address = " + cli.BaseAddress.ToString());

                var response = await cli.GetStringAsync("api/Values");
                logger.LogInformation("Resultado chamada = " + response);

                Msg = response.ToString();
                logger.LogInformation("Msg = " + Msg);
            }
            catch (Exception ex)
            {
                Msg += ex.ToString();
                logger.LogError("ERROR = " + ex.ToString());
            }
            return Page();
        }
    }
}