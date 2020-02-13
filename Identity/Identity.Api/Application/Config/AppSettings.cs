using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class AppSettings
    {
        public string TestConfig { get; set; }
        public SmtpConfig EmailConfig { get; set; }
    }
}
