using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Expenses.Core.Domain;
using Expenses.Core.Services;

namespace Expenses.App
{
    public partial class FormIncome : MetroFramework.Forms.MetroForm
    {
        private readonly IIncomeRepository _incomeRepository;
        private bool _hasLoaded = false;
        private List<Income> _incomes = new List<Income>();
        private EntityState _entityState = EntityState.Reading;

        public enum EntityState
        {
            Reading,
            Modifying,
            Resetting
        }

        public FormIncome(IIncomeRepository incomeRepository)
        {
            InitializeComponent();
            _incomeRepository = incomeRepository;
        }

        private void MakeDataSet(List<Income> incomes)
        {
            _entityState = EntityState.Resetting;
            _incomes = incomes;
            gridIncome.DataSource = null;
            gridIncome.DataSource = incomes;
        }

        private async void FormIncome_Load(object sender, EventArgs e)
        {
            await LoadIncomeData();
            gridIncome.AutoSizeRowsMode =
           DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            _hasLoaded = true;
        }

        private async Task LoadIncomeData()
        {
            try
            {
                var income = await _incomeRepository.GetAllAsync();
                MakeDataSet(income);
                CalculateTotalIncome(income);
                _entityState = EntityState.Reading;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTotalIncome(List<Income> incomes)
        {
            var totalIncome = incomes.Sum(x => x.Amount);
            lblTotalIncome.Text = totalIncome.ToString("c");
        }

        private async void btnIncome_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var income = new Income
                {
                    Amount = decimal.Parse(txtAmount.Text),
                    Name = txtName.Text,
                    Description = txtDescription.Text
                };

                try
                {
                    if (_entityState == EntityState.Reading)
                    {
                        await _incomeRepository.InsertAsync(income);
                    }
                    else
                    {
                        income.Id = int.Parse(lblId.Text);
                        await _incomeRepository.UpdateAsync(income);
                    }

                    await LoadIncomeData();
                    ClearForm();
                    MessageBox.Show("Operation was successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ClearForm()
        {
            txtAmount.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtName.Focus();
            btnIncome.Text = "Insert";
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
        }

        private async void metroButton2_Click(object sender, EventArgs e)
        {
            if (_entityState == EntityState.Modifying)
            {
                try
                {
                    await _incomeRepository.DeleteAsync(int.Parse(lblId.Text));
                    await LoadIncomeData();
                    ClearForm();
                    MessageBox.Show("Record successfully deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void gridIncome_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void gridIncome_SelectionChanged(object sender, EventArgs e)
        {
            if (_hasLoaded && _entityState != EntityState.Resetting)
            {
                var row = gridIncome.SelectedRows[0];
                if (int.TryParse(row.Cells[3].Value.ToString(), out int id))
                {
                    PopulateRecordForEdit(id);
                }
            }
        }

        private void PopulateRecordForEdit(int id)
        {
            var income = _incomes.FirstOrDefault(i => i.Id == id);
            if (income != null)
            {
                txtAmount.Text = income.Amount.ToString();
                txtDescription.Text = income.Description.ToString();
                txtName.Text = income.Name.ToString();
                lblId.Text = id.ToString();
                btnIncome.Text = "Update";
                btnDelete.Enabled = true;
                _entityState = EntityState.Modifying;
            }
        }
    }
}