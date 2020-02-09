using Identity.Api.Application.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.DTO
{
    public class LoginInputDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }

        public bool Validate(out ResultError errors)
        {
            errors = Errors.Validation;

            if (string.IsNullOrWhiteSpace(Username))
                errors.AddError(Errors.ValidationRequired(nameof(Username)));

            if (string.IsNullOrWhiteSpace(Password))
                errors.AddError(Errors.ValidationRequired(nameof(Password)));

            if (errors.Errors?.Count > 0)
                return false;

            return true;
        }
    }
}
