using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using itGlee.Controllers;
using itGlee.Models;
using System;

namespace itGlee.Pages
{
    [BindProperties]
    public class Apply : PageModel
    {
        [BindProperty]
        public Form form { get; set; }
        [BindProperty]
        public Job? applyingposition { get; set; }
        private List<Job> _jobsavailable;

        public Apply()
        {
            form = new Form();
            form.files = new List<IFormFile>();

            using StreamReader reader = new(Environment.CurrentDirectory + "/Careers.json");
            var json = reader.ReadToEnd();
            var jarray = JArray.Parse(json);
            _jobsavailable = new List<Job>();

            foreach (var item in jarray) _jobsavailable.Add(item.ToObject<Job>());
        }

        private Job? GetJobById(string id)
        {
            foreach (var job in _jobsavailable) if (job.id == id) return job;
            return null;
        }

        public IActionResult OnGet([FromQuery] string guid)
        {
            applyingposition = GetJobById(guid);
            return Page();
        }

        private (bool, string, string) CheckInputs(String? Email, String? Name, List<IFormFile>? files)
        {
            var emailChecker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email);
            if (Name == "" || Name == null) return (false, "name", "Please enter a name.");
            if (Name.Length >= 100) return (false, "name", "Your name exceeds allowed length. Please enter a name under 100 characters.");
            if (!emailChecker || Email == "" || Email == null) return (false, "emailaddress", "Invalid email address.");
            if (files == null || files.Count == 0) return (false, "files", "Please select a file to upload.");
            if (files.Count > 3) return (false, "files", "Please upload at most 3 files.");

            foreach (var file in files) if (file.Length >= (5 * 1024 * 1024)) return (false, "files", "Please upload file smaller than 5mb");

            return (true, "", "");
        }

        [HttpPost]
        public IActionResult OnPost()
        {

            JsonResult jr = new JsonResult(new { statusCode = 200, errorMessage = "" });
            // Validation of input
            form.name = Request.Form["form.name"];
            form.emailaddress = Request.Form["form.emailaddress"];
            form.files = new List<IFormFile>();
            applyingposition = GetJobById(Request.Form["applyingposition.id"]);

            EmailController ec = new EmailController();
            foreach (var file in Request.Form.Files) form.files.Add(file);

            (bool isValid, string key, string errorMessage) = CheckInputs(form.emailaddress, form.name, form.files);
            if (!isValid)
            {
                return new JsonResult(new { statusCode = 400, errorMessage = errorMessage });
            }

            try
            {
                ec.Send_v1(form.emailaddress, form.name, applyingposition.jobpositiondescription, form.files);
                return jr;
            }
            catch (Exception ex)
            {
                return new JsonResult(new { statusCode = 400, errorMessage = ex.Message });
            }
        }
    }
}