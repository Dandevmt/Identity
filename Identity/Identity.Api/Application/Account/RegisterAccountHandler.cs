using Identity.Api.Application.Config;
using Identity.Api.Application.Email;
using Identity.Api.DTO;
using Identity.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Application.Account
{
    public class RegisterAccountHandler
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailSender emailSender;
        

        public RegisterAccountHandler(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public async Task<Result<RegisterOutputDto>> Handle(RegisterInputDto input, string tokenUrlTemplate)
        {
            if (!input.Validate(out ResultError validationErrors))
                return Result<RegisterOutputDto>.Fail(validationErrors);

            var appUser = new AppUser() { UserName = input.Email, Email = input.Email };

            var result = await userManager.CreateAsync(appUser, input.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                {
                    return Result<RegisterOutputDto>.Fail(Errors.Validation().AddError(Errors.ValidationEmailTaken(nameof(input.Email), input.Email)));
                }
                var errors = result.Errors.Select(e => new ResultError(e.Code, e.Description));
                return Result<RegisterOutputDto>.Fail(Errors.IdentityError().AddErrors(errors));
            }

            string token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            token = HttpUtility.UrlEncode(token);
            string email = HttpUtility.UrlEncode(input.Email);
            string url = string.Format(tokenUrlTemplate, token, email);

            await emailSender.Send(new EmailMessage() { To = input.Email, From = "financials@ofbbutte.com", Subject = "Confirm Account", Body = $"<p>Thank you for registering. Please click the link (or copy into your browser) below to confirm your account.</p><br><br><a href='{url}'>{url}</a>" });
            // Add claims if needed
            //await userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("userName", user.UserName));
            //await userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("email", user.Email));


            return Result<RegisterOutputDto>.Success(new RegisterOutputDto());
        }
    }
}
