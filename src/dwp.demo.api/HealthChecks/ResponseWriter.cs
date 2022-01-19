using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace dwp.demo.api.HealthChecks
{
    public static class ResponseWriter
    {
        public static Func<HttpContext, HealthReport, Task> Write = async (context, report) =>
        {
            var result = System.Text.Json.JsonSerializer.Serialize(
                new
                {
                    Name = "dwp.demo.api",
                    Status = report.Status.ToString(),
                    Info = report.Entries.Select(e => new
                    {
                        Key = e.Key,
                        Description = e.Value.Description,
                        Status = Enum.GetName(typeof(HealthStatus),
                            e.Value.Status),
                        Error = e.Value.Exception?.Message
                    }).ToList()
                }, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        };
    }
}