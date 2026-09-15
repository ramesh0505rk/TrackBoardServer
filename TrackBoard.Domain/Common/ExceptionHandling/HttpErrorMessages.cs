using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Domain.Common.ExceptionHandling
{
    public static class HttpErrorMessages
    {
        public const string BadRequest = "An error occurred while processing your request. Please try again later.";
        public const string Unauthorized = "You are not authorized to access this resource. Please log in and try again.";
        public const string Forbidden = "You are not authorized to access this resource. Please log in and try again.";
        public const string InternalServerError = "An internal server error occurred. Please try again later.";
        public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    }
}
