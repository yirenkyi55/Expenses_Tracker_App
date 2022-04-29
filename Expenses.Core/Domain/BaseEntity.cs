using System;

namespace Expenses.Core.Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}