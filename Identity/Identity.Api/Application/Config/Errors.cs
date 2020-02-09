using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class Errors
    {
        public static ResultError IdentityError = new ResultError("3", "Identity Error");
        public static ResultError InvalidReturnUrl = new ResultError("4", "Invalid Return Url");
        public static ResultError InvalidCredentials = new ResultError("5", "Invalid Credentials");
        public static ResultError Validation = new ResultError("6", "Validation");
        public static FieldError ValidationRequired(string field) => new FieldError(field, "1", $"{field} is required");
        public static FieldError ValidationInvalidEmail(string field) => new FieldError(field, "2", "Invalid Email Adddress");
        public static FieldError ValidationMinLength(string field, int minLength) => new FieldError(field, "3", $"Must be at least {minLength} characters");
        public static FieldError ValidationMaxLength(string field, int maxLength) => new FieldError(field, "4", $"Must be less than {maxLength + 1} characters");
        public static FieldError ValidationPasswordMismatch(string field) => new FieldError(field, "5", "Password does not match");
    }
}
