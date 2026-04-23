using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace StBurger.App.Transformers;

public sealed class AcceptHeaderOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Parameters ??= [];

        var alreadyExists = operation.Parameters
            .Any(p => p.In == ParameterLocation.Header && p.Name == "Accept");

        if (!alreadyExists)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Default = "application/json"
                }
            });
        }

        return Task.CompletedTask;
    }
}