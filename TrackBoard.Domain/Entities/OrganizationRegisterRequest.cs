using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Domain.Entities
{
    public class OrganizationRegisterRequest
    {
        public string OrganizationName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        //public string OrganizationEmail { get; set; } = string.Empty;
        //public string OrganizationPhone { get; set; } = string.Empty;
        //public string OrganizationAddress { get; set; } = string.Empty;
        //public string AdminFirstName { get; set; } = string.Empty;
        //public string AdminLastName { get; set; } = string.Empty;
        //public string AdminEmail { get; set; } = string.Empty;
    }
}
