using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Application.ResponseDTOs
{
    public class BaseDTO
    {
        public string RequestId { get; set; }
        public string ResponseMessage { get; set; }
    }
}
