using System.ComponentModel.DataAnnotations;

namespace codenames;

public static class ValidationHelper
{
    public static bool IsValid<T>(T obj, out ICollection<ValidationResult> results) where T : class
    {
        var validationContext = new ValidationContext(obj);
        results = new List<ValidationResult>();

        return Validator.TryValidateObject(obj, validationContext, results, true);
    }
}