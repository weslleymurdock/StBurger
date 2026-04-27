namespace StBurger.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Executando {Request}", requestName);

        try
        {
            var response = await next(cancellationToken);

            logger.LogInformation("Execução de {Request} finalizada", requestName);

            return response;
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Erro na execução de {Request}.", requestName);
            throw;
        }
    }
}
