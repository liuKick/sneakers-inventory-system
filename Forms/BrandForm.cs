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
    public partial class BrandForm : Form
    {
        private BindingList<Brand> brands;
        private bool isEditing = false;
        private Brand currentBrand = null;

        public BrandForm()
        {
            InitializeComponent();
            brands = new BindingList<Brand>();

            // 🔥 ADDED SUPABASE INITIALIZATION 🔥
            SupabaseClient.Initialize();

            // 🔥 EVENT CONNECTIONS 🔥
            btnAdd.Click += btnAdd_Click;
            btnSave.Click += btnSave_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnSearch.Click += btnSearch_Click;
            btnClose.Click += btnClose_Click;

            // Enable scrolling for the form itself
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(800, 600);
        }

        private async void BrandForm_Load(object sender, EventArgs e)
        {
            await LoadBrandsAsync();
            SetupDataGridView();
            ResetForm();
        }

        private async Task LoadBrandsAsync()
        {
            try
            {
                var response = await SupabaseClient.Client.From<Brand>().Get();
                brands = new BindingList<Brand>(response.Models.ToList());

                dgvBrands.DataSource = null; // Clear first
                dgvBrands.DataSource = brands; // Re-bind
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading brands: {ex.Message}", "Error");
            }
        }

        private void SetupDataGridView()
        {
            dgvBrands.AutoGenerateColumns = false;
            dgvBrands.Columns.Clear();

            // Brand ID column (VISIBLE NOW) - CHANGED THIS SECTION
            dgvBrands.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colId",
                DataPropertyName = "Id",
                HeaderText = "Brand ID",
                Width = 120,
                Visible = true  // Changed to visible
            });

            // Brand Name column
            dgvBrands.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colBrandName",
                DataPropertyName = "BrandName",
                HeaderText = "Brand Name",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Created At column
            dgvBrands.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colCreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Created Date",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "yyyy-MM-dd HH:mm:ss"
                }
            });
        }

        private void ResetForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtCreatedAt.Text = "";

            isEditing = false;
            currentBrand = null;
            btnSave.Text = "Save";
        }

        private void UpdateFormWithSelectedBrand(Brand brand)
        {
            if (brand != null)
            {
                txtID.Text = brand.Id;
                txtName.Text = brand.BrandName;
                txtCreatedAt.Text = brand.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        // ===== BUTTON EVENT HANDLERS =====

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                if (isEditing && currentBrand != null)
                {
                    // Update existing brand
                    var brand = new Brand
                    {
                        Id = currentBrand.Id,
                        BrandName = txtName.Text.Trim(),
                        CreatedAt = currentBrand.CreatedAt // Keep original creation date
                    };

                    await SupabaseClient.Client.From<Brand>()
                        .Where(b => b.Id == currentBrand.Id)
                        .Update(brand);
                    MessageBox.Show("Brand updated successfully!", "Success");
                }
                else
                {
                    // Create new brand - let constructor generate ID and timestamp
                    var brand = new Brand
                    {
                        BrandName = txtName.Text.Trim()
                        // Id and CreatedAt are set in constructor
                    };

                    await SupabaseClient.Client.From<Brand>().Insert(brand);
                    MessageBox.Show("Brand added successfully!", "Success");
                }

                await LoadBrandsAsync();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving brand: {ex.Message}", "Error");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBrands.CurrentRow == null)
            {
                MessageBox.Show("Please select a brand to edit.", "Information");
                return;
            }

            var selectedBrand = dgvBrands.CurrentRow.DataBoundItem as Brand;
            if (selectedBrand != null)
            {
                currentBrand = selectedBrand;
                isEditing = true;

                UpdateFormWithSelectedBrand(selectedBrand);
                btnSave.Text = "Update";
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBrands.CurrentRow == null)
            {
                MessageBox.Show("Please select a brand to delete.", "Information");
                return;
            }

            var selectedBrand = dgvBrands.CurrentRow.DataBoundItem as Brand;
            if (selectedBrand != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {selectedBrand.BrandName}?",
                    "Confirm Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        await SupabaseClient.Client.From<Brand>()
                            .Where(b => b.Id == selectedBrand.Id)
                            .Delete();
                        MessageBox.Show("Brand deleted successfully!", "Success");
                        await LoadBrandsAsync();
                        ResetForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting brand: {ex.Message}", "Error");
                    }
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadBrandsAsync();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm))
            {
                _ = LoadBrandsAsync();
                return;
            }

            try
            {
                var filteredBrands = brands.Where(b =>
                    b.BrandName.ToLower().Contains(searchTerm)
                ).ToList();

                dgvBrands.DataSource = new BindingList<Brand>(filteredBrands);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error");
            }
        }

        // ===== VALIDATION =====

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter brand name.", "Validation Error");
                txtName.Focus();
                return false;
            }

            // Check for duplicate brand name (case insensitive)
            string newBrandName = txtName.Text.Trim();
            bool isDuplicate = brands.Any(b =>
                b.BrandName.Equals(newBrandName, StringComparison.OrdinalIgnoreCase) &&
                b.Id != (currentBrand?.Id ?? "")
            );

            if (isDuplicate)
            {
                MessageBox.Show("Brand name already exists. Please choose a different name.", "Validation Error");
                txtName.Focus();
                return false;
            }

            return true;
        }

        // ===== DATA GRID VIEW EVENTS =====

        private void dgvBrands_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBrands.CurrentRow != null && dgvBrands.CurrentRow.DataBoundItem is Brand selectedBrand)
            {
                UpdateFormWithSelectedBrand(selectedBrand);
            }
        }

        // ===== EMPTY EVENT HANDLERS FOR DESIGNER =====

        // 🔥 ADD THIS METHOD TO FIX THE ERROR 🔥
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Real-time search as user types
            btnSearch_Click(sender, e);
        }

        private void lblQuickActions_Click(object sender, EventArgs e) { }
        private void Panel_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}