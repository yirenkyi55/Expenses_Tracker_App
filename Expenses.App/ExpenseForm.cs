using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using Expenses.Core.Services;

using static Expenses.App.FormIncome;

namespace Expenses.App
{
    public partial class ExpenseForm : MetroFramework.Forms.MetroForm
    {
        private readonly IItemRepository _itemRepository;
        private readonly IExpenseRepository _expenseRepository;
        private bool _hasLoaded = false;
        private EntityState _entityState = EntityState.Reading;

        private List<Core.Domain.Item> _items = new List<Core.Domain.Item>();
        private List<Core.Domain.Expense> _expenses = new List<Core.Domain.Expense>();

        public ExpenseForm(IItemRepository itemRepository, IExpenseRepository expenseRepository)
        {
            InitializeComponent();
            _itemRepository = itemRepository;
            _expenseRepository = expenseRepository;
        }

        private async void ExpenseForm_Load(object sender, EventArgs e)
        {
            await LoadItemsAndExpenses();
        }

        private async Task LoadItemsAndExpenses()
        {
            var items = await _itemRepository.GetAllAsync();
            cboItem.DataSource = items;
            cboItem.DisplayMember = "Name";
            var expenses = await _expenseRepository.GetAllAsync();
            _expenses = expenses;
            MakeDataSet(expenses);
            CalculateTotalExpense(expenses);
            _entityState = EntityState.Reading;
        }

        private bool IsValid()
        {
            errorProvider1.Clear();

            if (cboItem.SelectedValue == null)
            {
                errorProvider1.SetError(cboItem, "Please select an item");
                return false;
            }

            if (!decimal.TryParse(txtCost.Text, out _))
            {
                errorProvider1.SetError(txtCost, "Invalid cost");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out _))
            {
                errorProvider1.SetError(txtCost, "Invalid quantity");
                return false;
            }

            return true;
        }

        private async Task LoadExpenseData()
        {
            try
            {
                var expenses = await _expenseRepository.GetAllAsync();
                MakeDataSet(expenses);
                CalculateTotalExpense(expenses);
                _entityState = EntityState.Reading;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTotalExpense(List<Core.Domain.Expense> expenses)
        {
            var totalExpense = expenses.Sum(x => x.Quantity * x.UnitCost);
            lblTotalExpenses.Text = totalExpense.ToString("c");
        }

        private void MakeDataSet(List<Core.Domain.Expense> expenses)
        {
            _entityState = EntityState.Resetting;
            _expenses = expenses;
            gridIncome.DataSource = null;
            gridIncome.DataSource = expenses;
        }

        private async void btnIncome_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var expense = new Core.Domain.Expense
                {
                    Quantity = int.Parse(txtQuantity.Text),
                    ItemId = (cboItem.SelectedItem as Core.Domain.Item).Id,
                    UnitCost = decimal.Parse(txtCost.Text),
                    Reason = txtReason.Text
                };

                try
                {
                    if (_entityState == EntityState.Reading)
                    {
                        await _expenseRepository.InsertAsync(expense);
                    }
                    else
                    {
                        expense.Id = int.Parse(lblId.Text);
                        await _expenseRepository.UpdateAsync(expense);
                    }

                    await LoadExpenseData();
                    ClearForm();
                    MessageBox.Show("Operation was successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            txtQuantity.Text = string.Empty;
            txtCost.Text = string.Empty;
            txtReason.Text = string.Empty;
            cboItem.Text = string.Empty;
            cboItem.Focus();
            btnIncome.Text = "Insert";
        }
    }
}