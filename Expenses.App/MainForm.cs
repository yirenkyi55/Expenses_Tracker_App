using System;
using System.Collections.Generic;

namespace Expenses.App
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        //private void CalculateTotalExpense(List<Core.Domain.Expense> expenses)
        //{
        //    var totalExpense = expenses.Sum(x => x.Quantity * x.UnitCost);
        //    lblTotalExpenses.Text = totalExpense.ToString("c");
        //}

        private void btnIncome_Click(object sender, EventArgs e)
        {
            var incomeForm = new FormIncome(AppService.Income);
            incomeForm.ShowDialog();
        }

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            var expenseForm = new ExpenseForm(AppService.Item, AppService.Expense);
            expenseForm.ShowDialog();
        }
    }
}