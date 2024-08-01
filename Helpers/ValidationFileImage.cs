using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Helpers
{
    public class ValidationFileImage : ValidationAttribute
    {
        private readonly string[] _validTypes = { ".jpg", ".jpeg", ".png", ".gif"};

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (!_validTypes.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    return new ValidationResult("Invalid image file type.");
                }
            }
            else
            {
                return new ValidationResult("No file provided.");
            }

            return ValidationResult.Success;
        }
    }
}
