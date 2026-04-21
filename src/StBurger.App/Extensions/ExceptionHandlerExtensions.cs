using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace StBurger.App.Extensions;

public static class ExceptionHandlerExtensions
{
    extension(IApplicationBuilder app)
    {
        internal void UseGlobalExceptionHandler()
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandler?.Error;

                    context.Response.ContentType = "application/json";
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
                        DomainExceptionBase e => new BaseResponse
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
                    await context.Response.WriteAsJsonAsync(response);

                });
            });
        }
    }
}
