using Identity.Api;
using Identity.Api.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.Api.Controllers
{
    public class AccountControllerTests :
    IClassFixture<WebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory<Startup>
            _factory;

        public AccountControllerTests(
            WebAppFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Post_Register()
        {
            var registerDto = new RegisterDto()
            {
                Email = "John@abcdef.com",
                Password = "P1@fon",
                ConfirmPassword = "P1@fon"
            };

            var httpResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/account/register")
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(registerDto), Encoding.UTF8, "application/json")
            });

            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var response = await httpResponse.Content.ReadAsStringAsync();
            var x = 20;
            //var response = JsonConvert.DeserializeObject<SignupResponse>(stringResponse);
            //Assert.Equal(_signupRequests[0].FullName, response.FullName);
            //Assert.Equal(_signupRequests[0].Email, response.Email);
            //Assert.Equal(_signupRequests[0].Role, response.Role);
            //Assert.True(Guid.TryParse(response.Id, out _));
        }
    }
}
