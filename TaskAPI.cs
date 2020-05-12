using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            
            return  newUser!= null
                ? (ActionResult)new OkObjectResult("Hello " + newUser.userName + "\n"+" specified description is : "+ newUser.description + "\n" + " your contact details are: "+ newUser.contactDetails+ "\n" + " your email ID is: " +newUser.email)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
