using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Domain.Common.ExceptionHandling
{
    public class BadRequestCustomException : Exception
    {
        public List<string> Errors { get; }
        public BadRequestCustomException(List<string> errors) : base("Validation failed.")
        {
            Errors = errors;
        }
    }

    public class UnAuthorizedCustomException : Exception
    {
        public List<string> Errors { get; }
        public UnAuthorizedCustomException(List<string> errors) : base("Unauthorized")
        {
            Errors = errors;
        }
    }
}
