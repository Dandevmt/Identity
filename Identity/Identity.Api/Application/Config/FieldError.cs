using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class FieldError : ResultError
    {
        public string Field { get; }

        public FieldError(string field, string code, string description) : base(code, description)
        {
            Field = field;
        }       
    }
}
