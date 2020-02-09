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

        public bool Validate(out ResultError errors)
        {
            errors = Errors.Validation;

            if (string.IsNullOrWhiteSpace(Email))
                errors.AddError(Errors.ValidationRequired(nameof(Email)));

            if (Email != null && !Email.Contains("@"))
                errors.AddError(Errors.ValidationInvalidEmail(nameof(Email)));

            if (string.IsNullOrWhiteSpace(Password))
                errors.AddError(Errors.ValidationRequired(nameof(Password)));

            if (Password != null && Password.Length < 7)
                errors.AddError(Errors.ValidationMinLength(nameof(Password), 7));

            if (ConfirmPassword != Password)
                errors.AddError(Errors.ValidationPasswordMismatch(nameof(ConfirmPassword)));

            if (errors.Errors?.Count > 0)
                return false;

            return true;
        }
    }
}
