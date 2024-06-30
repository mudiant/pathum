using Hatid.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Hatid.Data.Models
{
    public class User : IdentityUser<int>, ITrackable
    {       
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public override string UserName { get; set; } = string.Empty;
        public override string Email { get; set; } = string.Empty;
        public override string PhoneNumber { get; set; } = string.Empty;
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool LockoutEnabled { get; set; }
        public override int AccessFailedCount { get; set; }
        public DateTimeOffset LastLoginDate { get; set; } = DateTimeOffset.Now;
        public int LoginAttempts { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public int CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
        public int RoleId { get; set; }

        [NotMapped]
        public string Password { get; set; } = string.Empty;
        [NotMapped]
        public bool IsSendEmailInvite { get; set; } = false;
    }
}
