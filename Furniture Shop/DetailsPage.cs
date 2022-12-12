using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Furniture_Shop
{
    public partial class DetailsPage : MetroFramework.Forms.MetroForm
    {
        DatabaseDLL.Database database;
        string username, item_Name;
        public DetailsPage(string username, string item_Name)
        {
            InitializeComponent();
            this.Text = item_Name;
            this.Refresh();
            this.username = username;
            this.item_Name = item_Name;
            setData();
        }

        private void btn_Sell_Items_Click(object sender, EventArgs e)
        {
            string sel_Item_Type = null;
            if (rb_Offline.Checked)
                sel_Item_Type = "OFFLINE";
            else if (rb_Online.Checked)
                sel_Item_Type = "ONLINE";
            try
            {
                if (decimal.ToInt64(num_Items_Sold.Value) > int.Parse(lbl_Item_Count.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Items Sold Value Cannot Be Larger Than Items In Stock!");
                }
                else
                {
                    DialogResult confirmDelete = MessageBox.Show(this, "ARE YOU SURE YOU WANT TO DEDUCT " + num_Items_Sold.Value.ToString() + " ITEMS?", "SURE TO CONTINUE?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (confirmDelete == DialogResult.Yes)
                    {
                        DatabaseDLL.Database database = new DatabaseDLL.Database();
                        database.openConnection();
                        int itemID = database.getItemID(item_Name,lbl_Item_Code.Text);
                        database.addItemsSold(itemID, username, txt_Cus_Name.Text, txt_Cus_Contact_No.Text, DateTime.Now, num_Items_Sold.Value, sel_Item_Type);
                        database.substractItems(username, item_Name, lbl_Item_Supplier.Text, num_Items_Sold.Value);
                        database.closeConnection();
                        num_Items_Sold.Value = 1;
                        setData();
                    }
                }
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void btn_Remove_Iem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirmDelteItem = MessageBox.Show(this, "Sure You Want TO Delete This Item?", "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (confirmDelteItem == DialogResult.Yes)
                {
                    database = new DatabaseDLL.Database();
                    database.openConnection();
                    database.deleteItem(username, lbl_Item_Supplier.Text, item_Name);
                    database.closeConnection();
                    this.Close();
                }
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void btn_Update_Item_Click(object sender, EventArgs e)
        {
            database.openConnection();
            int item_ID = database.getItemID(item_Name, lbl_Item_Code.Text);
            database.closeConnection();
            Update_Item change_Price = new Update_Item(username,item_ID);
            change_Price.ShowDialog();
        }

        private void btn_View_Sold_Items_Click(object sender, EventArgs e)
        {
            database.openConnection();
            View_Sold_Items view_Sold_Items = new View_Sold_Items(database.getItemID(item_Name, lbl_Item_Code.Text),username);
            database.closeConnection();
            view_Sold_Items.ShowDialog();
        }

        public void setData()
        {
            try
            {
                database = new DatabaseDLL.Database();
                database.openConnection();
                var data = database.getDataAll(username, item_Name);
                if (File.Exists("./images/" + data.Item15.ToString()))
                    pb_Item_Image.Image = Image.FromFile("./images/" + data.Item15.ToString());
                else
                    pb_Item_Image.Image = Image.FromFile("./images/image-not-found-300x169.jpg");
                lbl_Item_Category.Text = data.Item1.ToString();
                lbl_Item_Supplier.Text = data.Item2.ToString();
                lbl_name.Text = data.Item3.ToString();
                lbl_Bought_Price.Text = data.Item4.ToString();
                lbl_Selling_Price.Text = data.Item5.ToString();
                lbl_Discount_Price.Text = data.Item6.ToString();
                lbl_Item_Count.Text = data.Item7.ToString();
                lbl_Total_Items_Purchased.Text = data.Item8.ToString();
                lbl_Bought_Cost.Text = data.Item9.ToString();
                lbl_Selling_Total.Text = data.Item10.ToString();
                lbl_Profit_Without_Discount.Text = data.Item11.ToString();
                lbl_ProfitWith_Discount.Text = data.Item12.ToString();
                lbl_Date_Of_Purchase.Text = data.Item13.ToString().Replace("12:00:00 AM", "");
                if (data.Item16.Length == 0)
                    lbl_Supplier_Warranty.Text = "NOT SPECIFIED";
                else
                    lbl_Supplier_Warranty.Text = data.Item16.ToString().Replace("12:00:00 AM","");
                lbl_Item_Code.Text = data.Item14.ToString();

                database.closeConnection();
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }
    }
}
