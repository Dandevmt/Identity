using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Application.Config
{
    public class Result<T>
    {
        public bool Succeeded { get; private set; }
        public T Value { get; private set; }
        private ICollection<ResultError> errors;
        public IReadOnlyCollection<ResultError> Errors { get { return errors?.ToList(); } }

        public IReadOnlyCollection<ResultError> AllErrors()
        {
            var errs = errors?.ToList() ?? new List<ResultError>();
            foreach(var err in Errors)
            {
                errs.AddRange(err.AllErrors());
            }
            return errs;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>() { Succeeded = true, Value = value };
        }

        public static Result<T> Fail(ResultError error)
        {
            return new Result<T>() { Succeeded = false, errors = new List<ResultError>() { error } };
        }

        public static Result<T> Fail(string errorCode, string errorDescription)
        {
            return Fail(new ResultError(errorCode, errorDescription));
        }

        public void AddError(ResultError error)
        {
            Succeeded = false;
            if (errors == null)
                errors = new List<ResultError>();

            errors.Add(error);
        }

        public void AddErrror(string errorCode, string errorDescription)
        {
            AddError(new ResultError(errorCode, errorDescription));
        }


    }
}
