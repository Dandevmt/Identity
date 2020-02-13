using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Email
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message);
    }
}
