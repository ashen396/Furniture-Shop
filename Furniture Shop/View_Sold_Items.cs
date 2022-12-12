using System;
using System.Data;

namespace Furniture_Shop
{
    public partial class View_Sold_Items : MetroFramework.Forms.MetroForm
    {
        int itemID = 0;
        string username = null;
        DatabaseDLL.Database database = null;
        DataTable dataTable = null;
        public View_Sold_Items(int itemID, string username)
        {
            InitializeComponent();
            database = new DatabaseDLL.Database();
            dataTable = new DataTable();
            this.itemID = itemID;
            this.username = username;
            getData(itemID, username);
        }

        public void getData(int itemID, string username)
        {
            try
            {
                dataTable.Clear();
                database.openConnection();
                database.getItemsSold(itemID, username).Fill(dataTable);
                lbl_Items_Sold_Count.Text = database.getItemsSoldCount(itemID, username).ToString();
                database.closeConnection();
                dt_Items_Sold.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void rb_Online_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dataTable.Clear();
                database.openConnection();
                database.getItemsSold(itemID, username, "ONLINE").Fill(dataTable);
                lbl_Items_Sold_Count.Text = database.getItemsSoldCount(itemID, username, "ONLINE").ToString();
                database.closeConnection();
                dt_Items_Sold.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void rb_Offline_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dataTable.Clear();
                database.openConnection();
                database.getItemsSold(itemID, username, "OFFLINE").Fill(dataTable);
                lbl_Items_Sold_Count.Text = database.getItemsSoldCount(itemID, username, "OFFLINE").ToString();
                database.closeConnection();
                dt_Items_Sold.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void rb_All_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dataTable.Clear();
                database.openConnection();
                database.getItemsSold(itemID, username).Fill(dataTable);
                lbl_Items_Sold_Count.Text = database.getItemsSoldCount(itemID, username).ToString();
                database.closeConnection();
                dt_Items_Sold.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }
    }
}
