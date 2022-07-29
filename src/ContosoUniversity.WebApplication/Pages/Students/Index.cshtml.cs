using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;

namespace ContosoUniversity.WebApplication.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory client;
        private TelemetryClient telemetry;

        public string CurrentFilter { get; set; }
        
        public IndexModel(IHttpClientFactory client, TelemetryClient telemetry)
        {
            this.client = client;
            this.telemetry = telemetry;
        }

        public Models.APIViewModels.StudentResult Student { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id, string SearchString)
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                var response = await client.CreateClient("client").GetStringAsync("api/Students");
                Student = JsonConvert.DeserializeObject<Models.APIViewModels.StudentResult>(response);
            }
            else
            {
                // Set up some properties and metrics:
                var properties = new Dictionary<string, string>
                    {{"action", "StudentSearch"}, {"filter", SearchString}};

                //var metrics = new Dictionary<string, double>
                //    {{"Score", currentGame.Score}, {"Opponents", currentGame.OpponentCount}};

                // Send the event:
                telemetry.TrackEvent("SearchStudent", properties);

                var response = await client.CreateClient("client").GetStringAsync("api/Students/Search?name=" + SearchString);
                Student = JsonConvert.DeserializeObject<Models.APIViewModels.StudentResult>(response);
            }

            return Page();
        }
    }
}