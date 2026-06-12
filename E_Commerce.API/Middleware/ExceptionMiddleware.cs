using E_Commerce.Application.Common;
using E_Commerce.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_Commerce.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Not found: {Message}", ex.Message);
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 404, 
                    Message = ex.Message,
                };
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning("Domain rule violation: {Message}", ex.Message);
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 400, 
                    Message = ex.Message,
                };
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                ErrorResponse errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Errors = ex.Errors.Select(e => e.ErrorMessage)
                };

                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);

            }
            catch (ConflictException ex)
            {
                _logger.LogWarning("Conflicting request: {Message}", ex.Message);
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 409,
                    Message = ex.Message
                };
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
                
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning("Unauthorized request: {Message}", ex.Message);
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 401,
                    Message = ex.Message
                };
                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Concurrency conflict on request {Path}", context.Request.Path);
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 409,
                    Message = "The resource was modified by another request. Please retry."
                };

                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for request {Path}", context.Request.Path);
                var isDevelopment = context.RequestServices
                    .GetRequiredService<IWebHostEnvironment>()
                    .IsDevelopment();

                ErrorResponse errorResponse = new ErrorResponse()
                {
                    StatusCode = 500,
                    Details = isDevelopment ? ex.Message : null
                };

                context.Response.StatusCode = errorResponse.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);

            }
        }
    }
}
