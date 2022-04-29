using System;

using Expenses.Core.Services;

namespace Expenses.App
{
    public partial class ExpenseForm : MetroFramework.Forms.MetroForm
    {
        private readonly IItemRepository _itemRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseForm(IItemRepository itemRepository, IExpenseRepository expenseRepository)
        {
            InitializeComponent();
            _itemRepository = itemRepository;
            _expenseRepository = expenseRepository;
        }

        private void ExpenseForm_Load(object sender, EventArgs e)
        {
        }
    }
}