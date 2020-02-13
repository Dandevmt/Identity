using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Api.Application.Account;
using Identity.Api.Application.Config;
using Identity.Api.Application.Email;
using Identity.Api.DTO;
using Identity.Api.ViewModels;
using Identity.Database;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly UserManager<AppUser> userManager;
        private readonly AppIdentityDbContext appIdentityDbContext;
        private readonly IEventService events;
        private readonly IEmailSender emailSender;

        public AccountController(
            SignInManager<AppUser> signInManager, 
            IIdentityServerInteractionService interaction, 
            IAuthenticationSchemeProvider schemeProvider, 
            UserManager<AppUser> userManager, 
            AppIdentityDbContext appIdentityDbContext, 
            IEventService events,
            IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.interaction = interaction;
            this.schemeProvider = schemeProvider;
            this.userManager = userManager;
            this.appIdentityDbContext = appIdentityDbContext;
            this.events = events;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            string tokenUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme) + "?token={0}&email={1}";
            var result = await new RegisterAccountHandler(userManager, emailSender).Handle(vm.Input, tokenUrl);

            if (result.Succeeded)
            {
                return View("RegistrationComplete", vm.Input.Email);
            }

            vm = new RegisterViewModel()
            {
                Input = new RegisterInputDto() { Email = vm.Input.Email },
                Output = result
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await new ConfirmEmailHandler(userManager, signInManager).Handle(token, email);

            return View(result);
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Username = GetUserName(returnUrl) ?? context?.LoginHint, // On signup populate with new account pre-populate with userName from client otherwise defer to IDS constext
                NewAccount = returnUrl.Contains("newAccount"),
            });
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputDto dto)
        {
            var result = await new LoginHandler(signInManager, userManager, interaction, events, Url)
                .Handle(dto);

            if (result.Succeeded)
            {
                return Redirect(result.Value.ReturnUrl);
            }               

            // something went wrong, show form 
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await signInManager.SignOutAsync();
            var context = await interaction.GetLogoutContextAsync(logoutId);
            return Redirect(context.PostLogoutRedirectUri);
        }

        private static string GetUserName(string returnUrl)
        {
            if (returnUrl == null)
                return null;
            const string parameter = "&userName=";
            return returnUrl.Contains("userName") ? returnUrl.Substring(returnUrl.IndexOf("&userName=") + parameter.Length) : null;
        }
    }
}