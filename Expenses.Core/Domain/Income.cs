namespace Expenses.Core.Domain
{
    public class Income : BaseEntity
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}