using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp
{
    public partial class ProductShopUI : Form
    {
        public ProductShopUI()
        {
            InitializeComponent();
            ShowOnListView();
        }

        private Product aProduct;
        string connectionString;
        private Product orderProduct;
        double totalPrice=0;

        void TakeInputTextBox()
        {
            aProduct = new Product();
            aProduct.id = Convert.ToInt32(productIdTextBox.Text);
            aProduct.name = productNameTextBox.Text;
            aProduct.price = Convert.ToDouble(productPriceTextBox.Text);
            aProduct.category = productCategoryTextBox.Text;
        }

        string ConenctionString()
        {
            string connection;
            connection = @"server=(Local)\sqlexpress;database=ProductDB;integrated Security=true";
            return connection;
        }

        string InsertIntoDatabase()
        {
            connectionString = ConenctionString();
            SqlConnection aConnection=new SqlConnection(connectionString);

            string query = string.Format("Insert Into Product_Table VALUES({0},'{1}',{2},'{3}')", aProduct.id, aProduct.name, aProduct.price, aProduct.category);
            SqlCommand aCommand=new SqlCommand(query,aConnection);
            aConnection.Open();
            int rowAffected = aCommand.ExecuteNonQuery();
            aConnection.Close();
            if (rowAffected > 0)
            {
                return "Data Inserted";
            }
            else
            {
                return " Already Inserted";
            }

        }

        Product GetProduct(int id)
        {
            Product getProduct=new Product();

            connectionString = ConenctionString();

            SqlConnection aConnection = new SqlConnection(connectionString);

            string query = string.Format("select * From Product_Table where (ProductId={0})", id);

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            List<Product> porductList = new List<Product>();
            aConnection.Open();
            SqlDataReader aReader = aCommand.ExecuteReader();

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Product showProduct = new Product();

                    getProduct.id = (int)aReader[1];
                    getProduct.name = aReader[2].ToString();
                    getProduct.price = (double)aReader[3];
                    getProduct.category = aReader[4].ToString();
                    
                }

            }
            aReader.Close();
            aConnection.Close();

            return getProduct;
        }

        void SaveOnListView(Product showProduct)
        {
            ListViewItem aItem = new ListViewItem();
            aItem.Text = showProduct.id.ToString();
            aItem.SubItems.Add(showProduct.name); 
            aItem.SubItems.Add(showProduct.price.ToString());
            aItem.SubItems.Add(showProduct.category);
            showProductListView.Items.Add(aItem);

        }
        void ShowOnListView()
        {
            showProductListView.Items.Clear();
            connectionString = ConenctionString();

            SqlConnection aConnection=new SqlConnection(connectionString);

            string query=string.Format("select* From Product_Table");

            SqlCommand aCommand=new SqlCommand(query,aConnection);
            List<Product>porductList=new List<Product>();
            aConnection.Open();
            SqlDataReader aReader = aCommand.ExecuteReader();

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Product showProduct=new Product();
                    
                    showProduct.id = (int) aReader[1];
                    showProduct.name = aReader[2].ToString();
                    showProduct.price = (double)aReader[3];
                    showProduct.category = aReader[4].ToString();
                    orderComboBox.Items.Add(showProduct.name);
                    SaveOnListView(showProduct);
                }
                
            }
            aConnection.Close();
        }

        void ClearTextBox()
        {
            productIdTextBox.Clear();
            productCategoryTextBox.Clear();
            productPriceTextBox.Clear();
            productNameTextBox.Clear();
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            int id = (Convert.ToInt32(productIdTextBox.Text));
            if(id==GetProduct(id).id)
            {
                MessageBox.Show("aleady exist");
            }
        else
            {
                TakeInputTextBox();
                MessageBox.Show(InsertIntoDatabase());
            }
            
            
            ShowOnListView();
            ClearTextBox();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            Product getProduct=new Product();

            int id = Convert.ToInt32(productIdTextBox.Text);
            getProduct=GetProduct(id);

            productNameTextBox.Text = getProduct.name;
            productPriceTextBox.Text = getProduct.price.ToString();
            productCategoryTextBox.Text = getProduct.category;

        }

        Product Search(string name)
        {

            Product getProduct = new Product();

            connectionString = ConenctionString();

            SqlConnection aConnection = new SqlConnection(connectionString);

            string query = string.Format("select * From Product_Table where (ProductName='{0}')", name);

            SqlCommand aCommand = new SqlCommand(query, aConnection);
            List<Product> porductList = new List<Product>();
            aConnection.Open();
            SqlDataReader aReader = aCommand.ExecuteReader();

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Product showProduct = new Product();

                    getProduct.id = (int)aReader[1];
                    getProduct.name = aReader[2].ToString();
                    getProduct.price = (double)aReader[3];
                    getProduct.category = aReader[4].ToString();

                }

            }
            aConnection.Close();

            return getProduct;
        }
        private void orderButton_Click(object sender, EventArgs e)
        {
            orderProduct = new Product();
            string name = orderComboBox.Text;
            int quantity = Convert.ToInt32(orderQuantityTextBox.Text);
            orderProduct = Search(name);
            
            
            ShowOnOrderListview(orderProduct,quantity);
            

        }

        void ShowOnOrderListview(Product orderProduct,int quantity)
        {
          
            
            double price = orderProduct.price*quantity;
            
            totalPrice+= price;
            ListViewItem aItem = new ListViewItem();
            aItem.Text = orderProduct.id.ToString();
            aItem.SubItems.Add(orderProduct.name);
            aItem.SubItems.Add((orderProduct.price).ToString());
            aItem.SubItems.Add(orderProduct.category);
            aItem.SubItems.Add(quantity.ToString());
            aItem.SubItems.Add(price.ToString());
            orderListView.Items.Add(aItem);
            totalPriceTextBox.Text = totalPrice.ToString();
        }

       private void orderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            orderQuantityTextBox.Clear();
            orderIdTextBox.Clear();
            string name = orderComboBox.Text;

            Product orProduct = Search(name);

            orderIdTextBox.Text = orProduct.price.ToString();

        }

        

        

       

       

        
    }
}
