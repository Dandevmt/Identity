using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Api.Application.Config
{
    public class ResultError
    {
        public ErrorCategory Category { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        public ResultError(ErrorCategory category, string code, string description)
        {
            this.Code = code;
            this.Description = description;
            this.Category = category;
        }
    }
}