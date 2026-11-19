using FluentValidation;
using LibraryService.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryService.Filters
{
    // ყოველი action-ის წინ ეძებს არგუმენტის Validator-ს (IValidator<T>) და უშვებს მას
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new Dictionary<string, List<string>>();

            // action-ის ყველა პარამეტრი: მაგ. (int id, UpdateBookRequest request)
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                // მაგ. UpdateBookRequest → IValidator<UpdateBookRequest>
                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

                // DI-ში ვეძებთ; int-ს და QueryParameters-ს ვალიდატორი არ აქვს → გამოვტოვებთ
                if (_serviceProvider.GetService(validatorType) is not IValidator validator)
                    continue;

                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                // ყველა დარღვეული წესი ერთად, არა მხოლოდ პირველი
                foreach (var failure in result.Errors)
                {
                    var key = ToCamelCase(failure.PropertyName);
                    if (!errors.TryGetValue(key, out var messages))
                    {
                        messages = new List<string>();
                        errors[key] = messages;
                    }
                    messages.Add(failure.ErrorMessage);
                }
            }

            if (errors.Count > 0)
            {
                // next()-ს არ ვიძახებთ → კონტროლერამდე მოთხოვნა ვერ მივა
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Validation failed",
                    Errors = errors.ToDictionary(e => e.Key, e => e.Value.ToArray())
                });
                return;
            }

            await next(); // ყველაფერი ვალიდურია → გადავდივართ action-ზე
        }

        private static string ToCamelCase(string key) =>
            string.IsNullOrEmpty(key) ? key : char.ToLowerInvariant(key[0]) + key.Substring(1);
    }
}