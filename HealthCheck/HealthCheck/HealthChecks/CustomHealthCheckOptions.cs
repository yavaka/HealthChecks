using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCheck.HealthChecks
{
    public class CustomHealthCheckOptions : HealthCheckOptions
    {
        public CustomHealthCheckOptions() : base()
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            ResponseWriter = async (c, r) =>
            {
                c.Response.ContentType = MediaTypeNames.Application.Json;
                c.Response.StatusCode = StatusCodes.Status200OK;

                var result = JsonSerializer.Serialize(new
                {
                    checks = r.Entries.Select(e => new 
                    {
                        //Identifying string in Startup.cs
                        name = e.Key,
                        //The whole duration of single check
                        responseTime = e.Value.Duration.TotalMilliseconds,
                        status = e.Value.Status.ToString(),
                        //The custom informative message configured in ICMPHealthCheck class
                        description = e.Value.Description
                    }),
                    // Healthy || Degraded || Unhealthy
                    totalStatus = r.Status,
                    // The whole duration of all the checks
                    totalResponseTime = r.TotalDuration.TotalMilliseconds
                }, jsonSerializerOptions);

                await c.Response.WriteAsync(result);
            };
        }
    }
}
