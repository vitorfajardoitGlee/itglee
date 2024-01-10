namespace itGlee.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Mail;
    using System.Net;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public class EmailController : Controller
    {
        private readonly IConfiguration Configuration;

        public EmailController()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("UserSecrets.json", optional: true, reloadOnChange: true).Build();
        }

        private IActionResult SendMessage(MailMessage message)
        {
            string address = Configuration.GetValue<string>("emailaddress");
            var password = Configuration.GetValue<string>("emailpassword");
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(address, password)
            };

            try
            {
                client.Send(message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(200);
        }

        private string EmailTemplateBody(string templatepath, Dictionary<string, string> dictionary) {
            string emailTemplateText = System.IO.File.ReadAllText(templatepath);
            foreach (var key in dictionary.Keys) {
                dictionary.TryGetValue(key, out string value);
                emailTemplateText = emailTemplateText.Replace(value,key);
            }
            return emailTemplateText;
        }

        private MailMessage SetMailMessage(string to, string to_name, string from, string from_name, string subject, bool IsBodyHtml) {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from, from_name);
            message.To.Add(new MailAddress(to, to_name));
            message.Subject = subject;
            message.IsBodyHtml = IsBodyHtml;

            return message;
        }

        private IActionResult SendNotificationToUser(string to, string from, string name, string position)
        {
            
            MailMessage message = SetMailMessage(to, name, from, "noreply", "Application received",true);
            message.Body = EmailTemplateBody(Directory.GetCurrentDirectory() + "\\Pages\\ApplicationReceived.html", new Dictionary<string, string> {{name,"&lt;%Name%&gt;"},{position, "&lt;%Position%&gt;"}});

            return SendMessage(message);
        }

        private IActionResult SendNewApplication(string to, string from, string name, string position, List<IFormFile> Files)
        {
            MailMessage message = SetMailMessage(to, "itGlee", from, name, "New application has been received.", true);

            using (var ms = new MemoryStream())
            {
                for (var i = 0; i < Files.Count; i++)
                {
                    Files[i].CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    message.Attachments.Add(new Attachment(new MemoryStream(fileBytes), Files[i].FileName));
                }
            }

            message.Body = EmailTemplateBody(Directory.GetCurrentDirectory() + "\\Pages\\NewApplication.html", new Dictionary<string, string> { { name, "&lt;%Name%&gt;" }, { position, "&lt;%Position%&gt;" } });
            return SendMessage(message);
        }

        public ObjectResult Send_v1(string Email, string Name, string Position, List<IFormFile> Files)
        {
            try
            {
                IActionResult usernotified = SendNotificationToUser(Email, Configuration.GetValue<string>("emailaddress"), Name, Position);
                if (usernotified.Equals(StatusCodes.Status500InternalServerError)) { return StatusCode(500, null); }
                IActionResult adminnotified = SendNewApplication(Configuration.GetValue<string>("emailaddress"), Email, Name, Position, Files);
                if (adminnotified.Equals(StatusCodes.Status500InternalServerError)) { return StatusCode(500, null); }

                return StatusCode(200, null);
            }
            catch (Exception e)
            {
                Exception ex = new Exception(string.Format("{0}", e.Message));
                throw ex;
            };
        }
    }
}
