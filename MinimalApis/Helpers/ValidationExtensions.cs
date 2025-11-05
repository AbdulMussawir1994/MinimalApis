using System.ComponentModel.DataAnnotations;

namespace MinimalApis.Helpers;


public static class ValidationExtensions
{
    public static IResult ValidateModel<T>(this T model)
    {
        if (model is null)
            return Results.BadRequest(new { error = "Request body is missing or invalid." });

        var context = new ValidationContext(instance: model);

        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            instance: context.ObjectInstance,
            validationContext: context,
            validationResults: results,
            validateAllProperties: true
        );

        if (!isValid)
        {
            var errors = results
                .SelectMany(vr => vr.MemberNames.Select(member => new { member, vr.ErrorMessage }))
                .GroupBy(e => e.member, e => e.ErrorMessage ?? "Invalid value")
                .ToDictionary(g => g.Key, g => g.ToArray());

            return Results.ValidationProblem(errors);
        }

        return Results.Ok();
    }
}