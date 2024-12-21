using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //dapper kullanımı için connection tanımlanması
        SqlConnection connection = new SqlConnection("Server=.\\SQLEXPRESS;initial Catalog=EgitimKampi501Db;integrated security=true");
        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO TblProduct (ProductName, ProductStock , ProductPrice , ProductCategory) VALUES (@productName, @productPrice, @productStock ,@productCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@productName", txtProductName.Text);
            parameters.Add("@productPrice", txtProductPrice.Text);
            parameters.Add("@productStock", txtProductStock.Text);
            parameters.Add("@productCategory", txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Yeni kitap ekleme işlemi başarılı");

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM TblProduct WHERE ProductId = @productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap silme işlemi başarılı");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "UPDATE TblProduct SET ProductName = @productName, ProductPrice = @productPrice, ProductStock = @productStock, ProductCategory = @productCategory WHERE ProductId = @productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", txtProductId.Text);
            parameters.Add("@productName", txtProductName.Text);
            parameters.Add("@productPrice", txtProductPrice.Text);
            parameters.Add("@productStock", txtProductStock.Text);
            parameters.Add("@productCategory", txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap güncelleme işlemi başarılı","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // toplam kitap sayısı
            string query1 = "Select Count(*) From TblProduct";
            var productTotalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.ToString();

            string query2 = "Select ProductName From TblProduct Where ProductPrice = (Select Max(ProductPrice) From TblProduct)";
            var maxPriceProduct = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxPriceProduct.Text = maxPriceProduct;


            string query3 = "Select Count(Distinct(ProductCategory)) From TblProduct";
            var distinctProductCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblDistinctCategoryCount.Text = distinctProductCount.ToString();
        }

    }
}
