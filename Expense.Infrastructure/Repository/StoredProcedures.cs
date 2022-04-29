namespace Expense.Infrastructure.Repository
{
    public static class StoredProcedures
    {
        public static class Item
        {
            public const string InsertItem = "sp_Item_Insert";
            public const string UpdateItem = "sp_Item_Update";
        }

        public static class Income
        {
            public const string InsertIncome = "sp_Income_Insert";
        }

        public static class Expense
        {
            public const string InsertExpense = "sp_Expense_Insert";
            public const string UpdateExpense = "sp_Expense_Update";
        }
    }
}