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

        public bool Validate(out ICollection<ResultError> errors)
        {
            errors = new List<ResultError>();

            if (string.IsNullOrWhiteSpace(Username))
                errors.Add(Errors.ValidationRequired(nameof(Username)));

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add(Errors.ValidationRequired(nameof(Password)));

            if (errors.Count > 0)
                return false;

            return true;
        }
    }
}
