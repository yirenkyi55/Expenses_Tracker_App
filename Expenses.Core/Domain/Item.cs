namespace Expenses.Core.Domain
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}