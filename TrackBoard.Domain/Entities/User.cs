using System;
using System.Collections.Generic;
using System.Text;

namespace TrackBoard.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid? OrgId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
