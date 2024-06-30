using System.ComponentModel.DataAnnotations;

namespace Hatid
{
    public enum EnumRole
    {        
        Admin = 1,
        Manager = 2,
        Sales = 3,
    }

    public enum EnumtDateFilter
    {
        [Display(Name = "This Month")]
        ThisMonth = 1,
        [Display(Name = "Last Month")]
        ThisWeek = 2,
        [Display(Name = "Custom Range")]
        CustomRange = 3
    }
}
