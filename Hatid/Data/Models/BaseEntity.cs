using Hatid.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatid.Data.Models
{
    public class BaseEntity : ITrackable
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTimeOffset? ModifiedDate { get; set; }
        public int CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
        
    }
}
