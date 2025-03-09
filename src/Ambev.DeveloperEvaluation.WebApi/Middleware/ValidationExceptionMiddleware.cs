﻿using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using System.Net.Mime;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var response = ApiResponse.CreateAsValidationError(ex.Errors.Select(error => (ValidationErrorDetail)error));
                await WriteResponseAsync(context, response);
            }
            catch (NotFoundDomainException ex)
            {
                var response = ApiResponse.CreateAsNotFound(ex.Error, ex.Detail);
                await WriteResponseAsync(context, response);
            }
            catch (DomainException ex)
            {
                var response = ApiResponse.CreateAsValidationError("Bussines rules failure", ex.Message);
                await WriteResponseAsync(context, response);
            }
        }

        private static Task WriteResponseAsync(HttpContext context, ApiResponse response)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
