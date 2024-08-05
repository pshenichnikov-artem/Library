using Library.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Library.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class GenreValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || ((List<string?>)value).Count == 0)
            {
                return ValidationResult.Success;
            }

            var genres = Enum.GetNames(typeof(GenreEnum))
                .Select(g => g.ToLower())
                .ToHashSet();

            var valueGenreList = ((List<string?>)value)
                .Select(x => x?.ToString().Replace(" ", "_").ToLower())
                .ToList();

            var formattedGenres = genres.Intersect(valueGenreList)
                .Select(x => char.ToUpper(x[0]) + x.Substring(1))
                .OrderBy(x => x)
                .ToList();

            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(validationContext.ObjectInstance, formattedGenres, null);
            }

            return ValidationResult.Success;
        }
    }
}
