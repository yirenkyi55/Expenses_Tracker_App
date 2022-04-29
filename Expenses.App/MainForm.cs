using System;

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

        private void btnIncome_Click(object sender, EventArgs e)
        {
            var incomeForm = new IncomeForm();
            incomeForm.ShowDialog();
        }

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            var expenseForm = new ExpenseForm(AppService.Item, AppService.Expense);
            expenseForm.ShowDialog();
        }
    }
}