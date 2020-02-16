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

        public static Result<T> Success(T value)
        {
            return new Result<T>() { Succeeded = true, Value = value };
        }

        public static Result<T> Fail(ResultError error)
        {
            return new Result<T>() { Succeeded = false, errors = new List<ResultError>() { error } };
        }

        public static Result<T> Fail(IEnumerable<ResultError> errors)
        {
            return new Result<T>() { Succeeded = false, errors = errors.ToList() };
        }

        public static Result<T> Fail(ErrorCategory category, string errorCode, string errorDescription)
        {
            return Fail(new ResultError(category, errorCode, errorDescription));
        }

        public Result<T> AddError(ResultError error)
        {
            Succeeded = false;
            if (errors == null)
                errors = new List<ResultError>();

            errors.Add(error);
            return this;
        }

        public Result<T> AddErrror(ErrorCategory category, string errorCode, string errorDescription)
        {
            return AddError(new ResultError(category, errorCode, errorDescription));
        }


    }
}
