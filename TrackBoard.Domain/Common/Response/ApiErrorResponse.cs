using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TrackBoard.Domain.Common.Response
{
    public class ApiErrorResponse
    {
        [JsonPropertyName("errors")]
        public ApiError Errors { get; set; }
    }

    public class ApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("details")]
        public string Details { get; set; }
    }
}
