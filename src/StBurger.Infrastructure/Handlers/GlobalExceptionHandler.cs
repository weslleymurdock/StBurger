using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StBurger.Application.Core.Responses;
using StBurger.Domain.Core.Exceptions;
using System.Net;
using System.Text.RegularExpressions;

namespace StBurger.Infrastructure.Handlers;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception");

        var (status, message) = exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            InvalidCastException => (StatusCodes.Status400BadRequest, exception.Message),
            ValidationException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ResourceGoneException => (StatusCodes.Status410Gone, exception.Message),
            TimeoutException => (StatusCodes.Status408RequestTimeout, exception.Message),
            TimeoutDomainException => (StatusCodes.Status408RequestTimeout, exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message),
            DependencyFailureException => (StatusCodes.Status424FailedDependency, exception.Message),
            DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx
                => sqlEx.Number switch
                {
                    2601 or 2627 => (StatusCodes.Status409Conflict, BuildDuplicateMessage(dbEx)),
                    547 => (StatusCodes.Status409Conflict, "Violação de integridade referencial."),
                    515 => (StatusCodes.Status400BadRequest, "Campo obrigatório não informado."),
                    2628 or 8152 => (StatusCodes.Status400BadRequest, "Valor excede o tamanho permitido."),
                    1205 => (StatusCodes.Status409Conflict, "Conflito de concorrência (deadlock)."),
                    -2 => (StatusCodes.Status408RequestTimeout, "Timeout ao acessar o banco de dados."),
                    _ => (StatusCodes.Status500InternalServerError, dbEx.Message)
                },
            DbUpdateException => (StatusCodes.Status500InternalServerError, exception.Message),
            DomainBaseException e => (e.Code, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, exception.Message)
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = exception.GetType().Name,
            Detail = message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = problem
        });

        return true;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number is 2601 or 2627;
        }

        return false;
    }

    private static string? ExtractDuplicateValue(string message)
    {
        var match = Regex.Match(message, @"\((.*?)\)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string BuildDuplicateMessage(DbUpdateException ex)
    {
        var rawMessage = ex.InnerException?.Message ?? ex.Message;

        var value = ExtractDuplicateValue(rawMessage);

        return value is not null
            ? $"O valor '{value}' já existe."
            : "Registro duplicado.";
    }
}