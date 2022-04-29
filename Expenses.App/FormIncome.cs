using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Expenses.Core.Domain;
using Expenses.Core.Services;

namespace Expenses.App
{
    public partial class FormIncome : MetroFramework.Forms.MetroForm
    {
        private readonly IIncomeRepository _incomeRepository;

        public FormIncome(IIncomeRepository incomeRepository)
        {
            InitializeComponent();
            _incomeRepository = incomeRepository;
        }

        private void MakeDataSet(List<Income> incomes)
        {
            gridIncome.DataSource = incomes;
        }

        private async void FormIncome_Load(object sender, EventArgs e)
        {
            await LoadIncomeData();
        }

        private async Task LoadIncomeData()
        {
            try
            {
                var income = await _incomeRepository.GetAllAsync();
                MakeDataSet(income);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnIncome_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                try
                {
                    var income = new Income
                    {
                        Amount = decimal.Parse(txtAmount.Text),
                        Name = txtName.Text,
                        Description = txtDescription.Text
                    };

                    await _incomeRepository.InsertAsync(income);
                    await LoadIncomeData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsValid()
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Name is required");
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                errorProvider1.SetError(txtAmount, "Invalid Amount");
                return false;
            }

            return true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
        }
    }
}