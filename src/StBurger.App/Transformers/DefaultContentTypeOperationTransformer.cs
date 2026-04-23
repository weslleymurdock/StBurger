using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace StBurger.App.Filters;

public sealed class DefaultContentTypeOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (operation.Responses is null)
            return Task.CompletedTask;

        foreach (var response in operation.Responses)
        {
            if (response.Value is null) continue;

            if (response.Value.Content is null) continue;

            var content = response.Value.Content;

            if (content.TryGetValue("application/json", out var jsonContent))
            {
                response.Value.Content.Clear();
                response.Value.Content["application/json"] = jsonContent;
            }
            else if (content.Any())
            {
                var first = content.First();

                response.Value.Content.Clear();
                response.Value.Content["application/json"] = first.Value;
            }
            else
            {
                response.Value.Content["application/json"] = new OpenApiMediaType();
            }
        }

        return Task.CompletedTask;
    }
}