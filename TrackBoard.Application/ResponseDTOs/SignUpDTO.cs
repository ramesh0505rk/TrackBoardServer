using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Application.ResponseDTOs
{
    public class SignUpDTO : BaseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
