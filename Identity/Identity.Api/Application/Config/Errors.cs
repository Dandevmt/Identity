using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class Errors
    {
        // ASPNET Identity
        public static ResultError IdentityError() => new ResultError(ErrorCategory.Identity, "3", "Identity Error");
        
        // Security
        public static ResultError InvalidReturnUrl() => new ResultError(ErrorCategory.Security, "4", "Invalid Return Url");
        public static ResultError InvalidCredentials() => new ResultError(ErrorCategory.Security, "5", "Invalid Credentials");
        public static ResultError EmailNotConfirmed() => new ResultError(ErrorCategory.Security, "7", "Email Not Confirmed");

        // Validation
        public static FieldError ValidationRequired(string field) => new FieldError(ErrorCategory.Validation, field, "1", $"{field} is required");
        public static FieldError ValidationInvalidEmail(string field) => new FieldError(ErrorCategory.Validation, field, "2", "Invalid Email Adddress");
        public static FieldError ValidationMinLength(string field, int minLength) => new FieldError(ErrorCategory.Validation, field, "3", $"Must be at least {minLength} characters");
        public static FieldError ValidationMaxLength(string field, int maxLength) => new FieldError(ErrorCategory.Validation, field, "4", $"Must be less than {maxLength + 1} characters");
        public static FieldError ValidationPasswordMismatch(string field) => new FieldError(ErrorCategory.Validation, field, "5", "Password does not match");
        public static FieldError ValidationMustContainUpper(string field) => new FieldError(ErrorCategory.Validation, field, "6", $"Must contain at least one upper case character");
        public static FieldError ValidationMustContainLower(string field) => new FieldError(ErrorCategory.Validation, field, "7", $"Must contain at least one lower case character");
        public static FieldError ValidationMustContainNumber(string field) => new FieldError(ErrorCategory.Validation, field, "8", $"Must contain at least one numerber");
        public static FieldError ValidationEmailTaken(string field, string value) => new FieldError(ErrorCategory.Validation, field, "9", $"{value} is already taken");
    }
}
