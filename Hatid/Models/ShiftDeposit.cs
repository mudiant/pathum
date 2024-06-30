namespace Hatid.Models
{
    public class ShiftDeposit
    {
        public int UserId { get; set; }
        public decimal DepositAmount { get; set; }
        public string UserName { get; set; }
        public ShiftDetail ShiftDetail { get; set; } = new();
    }

    public class ShiftDetail
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public double TotalShiftDuration { get; set; }
        public int ShiftCount { get; set; }
    }
}
