using SneakerShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SneakerShop.Forms
{
    public partial class CustomerForm : Form
    {
        private BindingList<Customer> customers;
        private bool isEditing = false;
        private Customer currentCustomer = null;

        public CustomerForm()
        {
            InitializeComponent();
            SupabaseClient.Initialize();
            customers = new BindingList<Customer>();

            // FIXED EVENT HANDLERS
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // Add purchase history button event
            this.btnViewPurchaseHistory.Click += new System.EventHandler(this.btnViewPurchaseHistory_Click);
        }

        private async void CustomerForm_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
            SetupDataGridView();
            SetupPurchaseHistoryGridView();
            ResetForm();
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                var response = await SupabaseClient.Client.From<Customer>().Get();
                var customerList = response.Models.ToList();
                customers = new BindingList<Customer>(customerList);
                dgvCustomers.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error");
            }
        }

        private void SetupDataGridView()
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.Columns.Clear();

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 120,
                Visible = false
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                Width = 150
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Phone",
                HeaderText = "Phone",
                Width = 120
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 200
            });
        }

        private void SetupPurchaseHistoryGridView()
        {
            dgvPurchaseHistory.AutoGenerateColumns = false;
            dgvPurchaseHistory.Columns.Clear();

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SaleDate",
                HeaderText = "Sale Date",
                Width = 120
            });

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ProductName",
                HeaderText = "Product",
                Width = 150
            });

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Quantity",
                HeaderText = "Qty",
                Width = 60
            });

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Unit Price",
                Width = 80
            });

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SubTotal",
                HeaderText = "Subtotal",
                Width = 80
            });

            dgvPurchaseHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TotalAmount",
                HeaderText = "Total Sale",
                Width = 90
            });
        }

        private void ResetForm()
        {
            txtID.Text = Guid.NewGuid().ToString();
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            isEditing = false;
            currentCustomer = null;
            btnSave.Text = "Save";

            // Clear purchase history
            dgvPurchaseHistory.DataSource = null;
            lblPurchaseHistoryTitle.Text = "PURCHASE HISTORY - Select a customer";
            UpdatePurchaseSummary(0, 0);
        }

        private void UpdatePurchaseSummary(int totalPurchases, decimal totalSpent)
        {
            lblPurchaseSummary.Text = $"Total Purchases: {totalPurchases} | Total Spent: ${totalSpent:F2}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                if (isEditing && currentCustomer != null)
                {
                    currentCustomer.Name = txtName.Text;
                    currentCustomer.Phone = txtPhone.Text;
                    currentCustomer.Email = txtEmail.Text;

                    await SupabaseClient.Client.From<Customer>()
                        .Where(c => c.Id == currentCustomer.Id)
                        .Update(currentCustomer);

                    MessageBox.Show("Customer updated successfully!", "Success");
                }
                else
                {
                    var customer = new Customer
                    {
                        Id = txtID.Text,
                        Name = txtName.Text,
                        Phone = txtPhone.Text,
                        Email = txtEmail.Text
                    };

                    await SupabaseClient.Client.From<Customer>().Insert(customer);
                    MessageBox.Show("Customer added successfully!", "Success");
                }

                await LoadCustomersAsync();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow?.DataBoundItem is Customer selectedCustomer)
            {
                currentCustomer = selectedCustomer;
                isEditing = true;
                txtID.Text = selectedCustomer.Id;
                txtName.Text = selectedCustomer.Name;
                txtPhone.Text = selectedCustomer.Phone;
                txtEmail.Text = selectedCustomer.Email;
                btnSave.Text = "Update";

                // Load purchase history for this customer
                LoadCustomerPurchaseHistory(selectedCustomer.Id);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow?.DataBoundItem is Customer selectedCustomer)
            {
                if (MessageBox.Show($"Delete {selectedCustomer.Name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await SupabaseClient.Client.From<Customer>().Where(c => c.Id == selectedCustomer.Id).Delete();
                    await LoadCustomersAsync();
                    ResetForm();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchTerm))
            {
                dgvCustomers.DataSource = customers;
                return;
            }

            var filtered = customers.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Phone.Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm)
            ).ToList();

            dgvCustomers.DataSource = new BindingList<Customer>(filtered);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async void btnViewPurchaseHistory_Click(object sender, EventArgs e)
        {
            if (currentCustomer != null)
            {
                await LoadCustomerPurchaseHistory(currentCustomer.Id);
            }
            else if (dgvCustomers.CurrentRow?.DataBoundItem is Customer selectedCustomer)
            {
                currentCustomer = selectedCustomer;
                await LoadCustomerPurchaseHistory(selectedCustomer.Id);
            }
            else
            {
                MessageBox.Show("Please select a customer to view purchase history.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task LoadCustomerPurchaseHistory(string customerId)
        {
            try
            {
                // Get all sales for this customer
                var salesResponse = await SupabaseClient.Client.From<Sale>()
                    .Where(s => s.CustomerId == customerId)
                    .Get();

                var sales = salesResponse.Models.ToList();

                var purchaseHistory = new List<PurchaseHistoryItem>();
                decimal totalSpent = 0;

                foreach (var sale in sales)
                {
                    // Get sale details for each sale
                    var detailsResponse = await SupabaseClient.Client.From<SaleDetail>()
                        .Where(sd => sd.SaleId == sale.Id)
                        .Get();

                    var saleDetails = detailsResponse.Models.ToList();

                    foreach (var detail in saleDetails)
                    {
                        // Get sneaker info
                        var sneakerResponse = await SupabaseClient.Client.From<Sneaker>()
                            .Where(sn => sn.Id == detail.SneakerId)
                            .Get();

                        var sneaker = sneakerResponse.Models.FirstOrDefault();

                        // FIX: Use the values directly (no null checking needed for decimal)
                        decimal unitPrice = detail.UnitPrice ?? 0; // This is correct for nullable
                        decimal subTotal = detail.SubTotal ?? 0;   // This is correct for nulla
                        decimal totalAmount = sale.TotalAmount;

                        // If unit price is 0 but we have quantity and subtotal, calculate it
                        if (unitPrice == 0 && detail.Quantity > 0 && subTotal > 0)
                        {
                            unitPrice = subTotal / detail.Quantity;
                        }

                        var historyItem = new PurchaseHistoryItem
                        {
                            SaleDate = sale.Date,
                            ProductName = sneaker?.Name ?? "Unknown Product",
                            Quantity = detail.Quantity,
                            UnitPrice = unitPrice,
                            SubTotal = subTotal,
                            TotalAmount = totalAmount
                        };

                        purchaseHistory.Add(historyItem);
                        totalSpent += subTotal;
                    }
                }

                // Display purchase history
                dgvPurchaseHistory.DataSource = purchaseHistory;
                lblPurchaseHistoryTitle.Text = $"PURCHASE HISTORY - {currentCustomer?.Name} ({purchaseHistory.Count} purchases)";
                UpdatePurchaseSummary(purchaseHistory.Count, totalSpent);

                if (purchaseHistory.Count == 0)
                {
                    MessageBox.Show("No purchase history found for this customer.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history: {ex.Message}", "Error");
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter name.", "Error");
                return false;
            }
            return true;
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (!isEditing && dgvCustomers.CurrentRow?.DataBoundItem is Customer customer)
            {
                txtID.Text = customer.Id;
                txtName.Text = customer.Name;
                txtPhone.Text = customer.Phone;
                txtEmail.Text = customer.Email;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }
    }

    // Purchase History Item class for display
    public class PurchaseHistoryItem
    {
        public DateTime SaleDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
    }
}