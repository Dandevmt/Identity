using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class FieldError : ResultError
    {
        public string Field { get; }

        public FieldError(ErrorCategory category, string field, string code, string description) : base(category, code, description)
        {
            Field = field;
        }       
    }
}
