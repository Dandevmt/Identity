using Identity.Api.Application.Config;
using Identity.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterInputDto Input { get; set; }
        public Result<RegisterOutputDto> Output { get; set; }
    }
}
