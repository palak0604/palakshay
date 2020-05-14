using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PalakshayAPI
{
    public static class TaskAPI
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "TaskAPI")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<NewUser>(requestBody);
            var newUser = new NewUser()
            {
                userName = data.userName,
                description = data.description,
                contactDetails = data.contactDetails,
                email = data.email
            };
            string to = "palakgupta0604@gmail.com"; 
            string from = newUser.email;  
            MailMessage message = new MailMessage(from, to);

            string mailbody = newUser.description + "\n" + " Contact Details : " + newUser.contactDetails;
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(to, "Enter gmail account password here.");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return  newUser!= null
                ? (ActionResult)new OkObjectResult("Hello " + newUser.userName + "\n"+" specified description is : "+ newUser.description + "\n" + " your contact details are: "+ newUser.contactDetails+ "\n" + " your email ID is: " +newUser.email)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
