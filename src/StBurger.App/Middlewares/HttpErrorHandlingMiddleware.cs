namespace StBurger.App.Middlewares;

public class HttpErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        // Intercepta respostas com status code de erro e formata a resposta de acordo
        if (context.Response.StatusCode >= 400)
        {
            // Evita sobrescrever respostas já tratadas pelo ExceptionHandler
            if (context.Response.HasStarted) return;

            context.Response.ContentType = "application/json";

            var response = context.Response.StatusCode switch
            {
                400 => new BaseResponse { Code = 400, Message = "Requisição inválida (Bad Request)." },
                404 => new BaseResponse { Code = 404, Message = "Recurso não encontrado." },
                405 => new BaseResponse { Code = 405, Message = "Método HTTP não permitido." },
                406 => new BaseResponse { Code = 406, Message = "Formato de resposta não aceitável." },
                408 => new BaseResponse { Code = 408, Message = "Tempo de requisição excedido (Timeout)." },
                409 => new BaseResponse { Code = 409, Message = "Conflito de recurso." },
                410 => new BaseResponse { Code = 410, Message = "Recurso não está mais disponível (Gone)." },
                415 => new BaseResponse { Code = 415, Message = "Tipo de mídia não suportado." },
                422 => new BaseResponse { Code = 422, Message = "Entidade não processável (Unprocessable Entity)." },
                429 => new BaseResponse { Code = 429, Message = "Muitas requisições (Too Many Requests)." },
                _ => new BaseResponse { Code = context.Response.StatusCode, Message = "Erro inesperado." }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
