using Identity.Api.Application.Config;
using Identity.Api.Application.DTO;
using Identity.Api.DTO;
using Identity.Api.ViewModels;
using Identity.Database;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Account
{
    public class LoginHandler
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IUrlHelper url;

        public LoginHandler(
            SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interaction,
            IEventService events,
            IUrlHelper url)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.interaction = interaction;
            this.events = events;
            this.url = url;
        }

        public async Task<Result<LoginOutputDto>> Handle(LoginInputDto dto)
        {
            if (!dto.Validate(out ResultError validationErrors))
                return Result<LoginOutputDto>.Fail(validationErrors);

            // check if we are in the context of an authorization request
            var context = await interaction.GetAuthorizationContextAsync(dto.ReturnUrl);

            // validate username/password
            var user = await userManager.FindByNameAsync(dto.Username);

            if (user != null && await userManager.CheckPasswordAsync(user, dto.Password))
            {
                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (AccountOptions.AllowRememberLogin && dto.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                    };
                };

                // issue authentication cookie with subject ID and username
                await signInManager.SignInAsync(user, true);
                //await HttpContext.SignInAsync(user.Id, user.UserName, props);

                if (context != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Result<LoginOutputDto>.Success(new LoginOutputDto(dto.ReturnUrl));
                }

                // request for a local page
                if (url.IsLocalUrl(dto.ReturnUrl))
                {
                    return Result<LoginOutputDto>.Success(new LoginOutputDto(dto.ReturnUrl));
                }
                else if (string.IsNullOrEmpty(dto.ReturnUrl))
                {
                    return Result<LoginOutputDto>.Success(new LoginOutputDto("~/"));
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    return Result<LoginOutputDto>.Fail(Errors.InvalidReturnUrl());
                }
            }

            await events.RaiseAsync(new UserLoginFailureEvent(dto.Username, "invalid credentials"));
            return Result<LoginOutputDto>.Fail(Errors.InvalidCredentials());
        }
    }
}
