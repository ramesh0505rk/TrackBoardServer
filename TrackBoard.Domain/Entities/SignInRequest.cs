using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Domain.Entities
{
    public class SignInRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
