
using Identity.Api.Application.Config;
using Identity.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Account
{
    public class ConfirmEmailHandler
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public ConfirmEmailHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<Result<bool>> Handle(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<bool>.Fail(Errors.ValidationInvalidEmail(nameof(email)));
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ResultError(e.Code, e.Description));
                return Result<bool>.Fail(Errors.IdentityError().AddErrors(errors));
            }

            // Sign the user in
            await signInManager.SignInAsync(user, true);

            return Result<bool>.Success(true);
        }
    }
}
