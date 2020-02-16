using Identity.Api.Application.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.DTO
{
    public class RegisterInputDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool Validate(out ICollection<ResultError> errors)
        {
            errors = new List<ResultError>();

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add(Errors.ValidationRequired(nameof(Email)));

            if (Email != null && !Email.Contains("@"))
                errors.Add(Errors.ValidationInvalidEmail(nameof(Email)));            

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add(Errors.ValidationRequired(nameof(Password)));

            if (Password != null && Password.Length < 7)
                errors.Add(Errors.ValidationMinLength(nameof(Password), 7));

            if (Password != null && !Password.Any(c => char.IsUpper(c)))
                errors.Add(Errors.ValidationMustContainUpper(nameof(Password)));

            if (Password != null && !Password.Any(c => char.IsLower(c)))
                errors.Add(Errors.ValidationMustContainLower(nameof(Password)));

            if (Password != null && !Password.Any(c => char.IsNumber(c)))
                errors.Add(Errors.ValidationMustContainNumber(nameof(Password)));

            if (ConfirmPassword != Password)
                errors.Add(Errors.ValidationPasswordMismatch(nameof(ConfirmPassword)));

            if (errors.Count > 0)
                return false;

            return true;
        }
    }
}
