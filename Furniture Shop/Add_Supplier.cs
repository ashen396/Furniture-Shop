using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Furniture_Shop
{
    public partial class Add_Supplier : MetroFramework.Forms.MetroForm
    {
        string username;
        public Add_Supplier(string username)
        {
            InitializeComponent();
            lbl_username.Text = username;
            this.username = username;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_Supplier_Name.Text.Length == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Supplier Name Cannot Be Blank!");
            }
            else
            {
                try
                {
                    DatabaseDLL.Database database = new DatabaseDLL.Database();
                    database.openConnection();
                    database.addSupplier(username, txt_Supplier_Name.Text, txt_Supplier_Contact_Number.Text, txt_Supplier_Email.Text, txt_Supplier_Address.Text);
                    database.closeConnection();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, ex.Message);
                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_Supplier_Contact_Number_Leave(object sender, EventArgs e)
        {
            if (txt_Supplier_Contact_Number.Text.Length > 0)
            {
                if (txt_Supplier_Contact_Number.Text.ToString().Any(char.IsLetter))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Contact Number Cannot Contain Letters!");
                }else if(txt_Supplier_Contact_Number.Text.Length >= 10)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Contact Number Cannot Have Digits More Than 10!");
                }
            }
        }

        private void txt_Supplier_Email_Leave(object sender, EventArgs e)
        {
            if(txt_Supplier_Email.Text.Length > 0)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(txt_Supplier_Email.Text);
                if (!match.Success)
                    MetroFramework.MetroMessageBox.Show(this, "Enter A Valid Email!");
            }
        }
    }
}
