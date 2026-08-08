using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Pokemon.Web.Models;

namespace Pokemon.Web.Filters;

public class SwaggerResponseFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var errorResponse = new OpenApiResponse
        {
            Description = "Request error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository)
                }
            }
        };

        if (!operation.Responses.ContainsKey("400"))
        {
            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request - Validation error",
                Content = errorResponse.Content
            });
        }

        if (!operation.Responses.ContainsKey("401"))
        {
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized",
                Content = errorResponse.Content
            });
        }

        if (!operation.Responses.ContainsKey("403"))
        {
            operation.Responses.Add("403", new OpenApiResponse
            {
                Description = "Forbidden - Insufficient permissions",
                Content = errorResponse.Content
            });
        }

        if (!operation.Responses.ContainsKey("404"))
        {
            operation.Responses.Add("404", new OpenApiResponse
            {
                Description = "Resource not found",
                Content = errorResponse.Content
            });
        }

        if (!operation.Responses.ContainsKey("500"))
        {
            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Internal server error",
                Content = errorResponse.Content
            });
        }

        if (!operation.Responses.ContainsKey("504"))
        {
            operation.Responses.Add("504", new OpenApiResponse
            {
                Description = "Gateway timeout",
                Content = errorResponse.Content
            });
        }
    }
}