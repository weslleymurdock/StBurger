using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace StBurger.App.Handlers;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var response = exception switch
        {
            // Erros de validação (422 Unprocessable Entity)
            ValidationException e => new BaseResponse
            {
                Code = 422,
                Message = $"Erro de validação: {e.Message}"
            },

            // Recurso não encontrado (404 Not Found)
            KeyNotFoundException e => new BaseResponse
            {
                Code = 404,
                Message = $"Recurso não encontrado: {e.Message}"
            },

            // Recurso não disponível (410 Gone)
            ResourceGoneException e => new BaseResponse
            {
                Code = 410,
                Message = $"Recurso indisponível: {e.Message}"
            },

            // Timeout (408 Request Timeout)
            TimeoutException e => new BaseResponse
            {
                Code = 408,
                Message = "Tempo de requisição excedido."
            },

            TimeoutDomainException e => new BaseResponse
            {
                Code = 408,
                Message = "Tempo de requisição excedido."
            },

            // Requisição inválida (400 Bad Request)
            ArgumentException e => new BaseResponse
            {
                Code = 400,
                Message = $"Parâmetro inválido: {e.Message}"
            },

            // Conflito (409 Conflict)
            ConflictException e => new BaseResponse
            {
                Code = 409,
                Message = $"Conflito de recurso: {e.Message}"
            },

            // Dependência falhou (424 Failed Dependency)
            DependencyFailureException e => new BaseResponse
            {
                Code = 424,
                Message = $"Falha em dependência: {e.Message}"
            },

            // Erros de banco de dados (500 Internal Server Error)
            DbUpdateException e => new BaseResponse
            {
                Code = 500,
                Message = "Erro ao atualizar o banco de dados."
            },

            // Exceções de domínio
            DomainBaseException e => new BaseResponse
            {
                Code = e.Code,
                Message = e.Message
            },


            // Erros não tratados
            _ => new BaseResponse
            {
                Code = 500,
                Message = $"Ocorreu um erro inesperado. :{exception?.Message}"
            }
        };

        var problem = new ProblemDetails
        {
            Status = response.Code,
            Title = "Error",
            Detail = response.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = response.Code;

        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}