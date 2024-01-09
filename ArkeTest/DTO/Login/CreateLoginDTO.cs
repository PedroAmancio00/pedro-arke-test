using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ArkeTest.DTO.Login
{
    public class CreateLoginDTO
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [MinLength(8)]
        [PasswordComplexity]
        public string Password { get; set; } = null!;
    }


    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Password is required.");
            }

            string? password = value.ToString();

#pragma warning disable CS8604 // Possible null reference argument.
            if (!Regex.IsMatch(password, @"(?=.*\p{Lu})") ||         // At least one upper case
                !Regex.IsMatch(password, @"(?=.*\p{Ll})") ||         // At least one lower case
                !Regex.IsMatch(password, @"(?=.*\d)") ||             // At least one digit
                !Regex.IsMatch(password, @"(?=.*[^\p{L}\p{Nd}])"))   // At least one special character
            {
                return new ValidationResult("Password must be at least 8 characters long and contain at least one upper case letter, one lower case letter, one digit, and one special character.");
            }
#pragma warning restore CS8604 // Possible null reference argument.


            return ValidationResult.Success;
        }
    }
}


