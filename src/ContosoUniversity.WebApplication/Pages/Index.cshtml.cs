using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public IConfiguration Configuration { get; }


        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
            var section = Configuration.GetSection("Infos");
            ViewData["Ambiente"] = section["Ambiente"].ToString();
            ViewData["Versao"] = section["Versao"].ToString();
        }
    }
}