using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

public static class RolesApi
{
    [FunctionName("roles")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        // Get client principal from headers (Static Web Apps injects this)
        var principalHeader = req.Headers["x-ms-client-principal"].FirstOrDefault();
        if (string.IsNullOrEmpty(principalHeader))
        {
            return new UnauthorizedObjectResult(new { error = "Unauthorized" });
        }

        // Decode and parse roles (for demo, just return the header value)
        // In production, decode the header and extract roles
        // See: https://learn.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=csharp
        var roles = new List<string>();
        // ...parse roles from principalHeader...

        return new OkObjectResult(new { roles });
    }
}
