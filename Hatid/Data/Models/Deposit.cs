using Hatid.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Hatid.Data.Models
{
    public class Deposit : BaseEntity
    {       
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DepositDate { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        
    }
}
