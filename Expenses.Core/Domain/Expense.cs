namespace Expenses.Core.Domain
{
    public class Expense : BaseEntity
    {
        public string Reason { get; set; }
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
}