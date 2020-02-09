using Identity.Api.Application.Config;
using Identity.Api.DTO;
using Identity.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Identity.Api.Application.Account
{
    public class RegisterAccountHandler
    {
        private readonly UserManager<AppUser> userManager;

        public RegisterAccountHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Result<RegisterOutputDto>> Handle(RegisterInputDto input)
        {
            if (!input.Validate(out ResultError validationErrors))
                return Result<RegisterOutputDto>.Fail(validationErrors);

            var appUser = new AppUser() { UserName = input.Email, Email = input.Email };

            var result = await userManager.CreateAsync(appUser, input.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ResultError(e.Code, e.Description));
                return Result<RegisterOutputDto>.Fail(Errors.IdentityError.AddErrors(errors));
            }

            // Add claims if needed
            //await userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("userName", user.UserName));
            //await userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("email", user.Email));


            return Result<RegisterOutputDto>.Success(new RegisterOutputDto());
        }
    }
}
