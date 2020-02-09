using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Api.Application.Config
{
    public class ResultError
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        private ICollection<ResultError> errors;
        public IReadOnlyCollection<ResultError> Errors { get { return errors.ToList(); } }

        public ResultError(string code, string description)
        {
            this.Code = code;
            this.Description = description;
            this.errors = new List<ResultError>();
        }

        public ResultError(string code, string description, IEnumerable<ResultError> childErrors)
        {
            this.Code = code;
            this.Description = description;
            this.errors = childErrors.ToList();
        }        

        public ResultError AddError(ResultError error)
        {
            if (errors == null)
                errors = new List<ResultError>();

            errors.Add(error);

            return this;
        }

        public ResultError AddError(string code, string description)
        {
            return AddError(new ResultError(code, description));
        }

        public ResultError AddErrors(IEnumerable<ResultError> errors)
        {
            foreach(var error in errors)
            {
                this.errors.Add(error);
            }
            return this;
        }
    }
}