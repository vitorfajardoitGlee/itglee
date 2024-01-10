using itGlee.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace itGlee.Pages
{
    public class IndexModel : PageModel
    {
        public readonly string title;
        public readonly string url;
        public readonly string email;
        public readonly string phone;

        public IndexModel()
        {
            title = "itGlee - Agile with a Smile Solutions";
            url = (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") ? "https://localhost:7172" : "https://www.itglee.com";
            email = "info@itglee.com";
            phone = "(+351) 233 094 915";
        }

        public void OnGet()
        {

        }
    }
}