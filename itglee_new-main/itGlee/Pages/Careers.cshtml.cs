using itGlee.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace itGlee.Pages
{
    public class Career : PageModel
    {
        public readonly string title;
        public readonly string url;
        public readonly List<Job>? jobs;
        public List<Job> careers;


        public Career(ILogger<Career> logger)
        {
            title = "Careers";
            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") { url = "https://localhost:7172"; }
            else { url = "https://www.itglee.com"; }

            careers = new List<Job>();
        }

        public void OnGet()
        {
            using StreamReader reader = new(Environment.CurrentDirectory + "/Careers.json");
            var json = reader.ReadToEnd();
            var jarray = JArray.Parse(json);
            foreach (var item in jarray)
            {
                Job? myjob = item.ToObject<Job>();
                careers.Add(myjob);
            }
        }
    }
}