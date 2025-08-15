using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.Json;

public static class RolesApi
{
    [FunctionName("roles")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Roles API called");

        // Get client principal from headers (Static Web Apps injects this)
        var principalHeader = req.Headers["x-ms-client-principal"].FirstOrDefault();

        if (string.IsNullOrEmpty(principalHeader))
        {
            log.LogWarning("No x-ms-client-principal header found");
            // Return empty roles instead of 401 - let SWA handle authentication
            return new OkObjectResult(new { roles = new string[0] });
        }

        try
        {
            // Decode the base64 principal header
            var principalBytes = Convert.FromBase64String(principalHeader);
            var principalJson = Encoding.UTF8.GetString(principalBytes);
            var principal = JsonSerializer.Deserialize<ClientPrincipal>(principalJson);

            log.LogInformation($"User authenticated: {principal?.userDetails}");

            // For now, assign a default role to all authenticated users
            // You can customize this logic based on your requirements
            var roles = new List<string> { "authenticated" };

            // Example: Add admin role for specific users
            if (principal?.userDetails?.Contains("admin") == true)
            {
                roles.Add("admin");
            }

            return new OkObjectResult(new { roles });
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing client principal: {ex.Message}");
            return new OkObjectResult(new { roles = new string[0] });
        }
    }
}

// Model for deserializing the client principal
public class ClientPrincipal
{
    public string identityProvider { get; set; }
    public string userId { get; set; }
    public string userDetails { get; set; }
    public IEnumerable<string> userRoles { get; set; }
}
