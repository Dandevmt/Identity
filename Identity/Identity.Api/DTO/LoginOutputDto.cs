namespace Identity.Api.Application.DTO
{
    public class LoginOutputDto
    {
        public string ReturnUrl { get; }

        public LoginOutputDto(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
    }
}