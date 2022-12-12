using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DatabaseDLL;

namespace Furniture_Shop
{
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        DatabaseDLL.Database database = new DatabaseDLL.Database();
        public Login()
        {
            InitializeComponent();
            try
            {
                if (File.Exists("./config"))
                {
                    StreamReader reader = new StreamReader("./config");
                    String computer_name = reader.ReadLine();
                    SqlConnection con = new SqlConnection("Data Source=" + computer_name + "\\SQLEXPRESS;Initial Catalog=Furniture_Shop;Integrated Security=True");
                }
                else
                {
                    Get_Connection_String get_Connection_String = new Get_Connection_String();
                    get_Connection_String.ShowDialog();
                }
            }
            catch (FileNotFoundException ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void lbl_register_Click(object sender, EventArgs e)
        {
            lbl_register.Hide();
            this.Text = "REGISTER";
            panel_register.Visible = true;
            this.Refresh();
        }

        private void txt_last_name_Leave(object sender, EventArgs e)
        {
            if (txt_last_name.Text.Length > 0)
            {
                if (txt_last_name.Text.ToString().Any(char.IsDigit) || txt_last_name.Text.ToString().Any(char.IsWhiteSpace))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please Check Name!");
                    txt_last_name.Clear();
                }
                else
                    lbl_username.Text = txt_first_name.Text + "_" + txt_last_name.Text;
            }
            else
                MetroFramework.MetroMessageBox.Show(this, "Name Cannot Be Blank!");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            clearandcloseregistrationpanel();
        }

        public void clearandcloseregistrationpanel()
        {
            lbl_register.Show();
            this.Text = "LOGIN";
            panel_register.Visible = false;
            this.Refresh();
            txt_first_name.Clear();
            txt_last_name.Clear();
            txt_age.Clear();
            txt_address.Clear();
            txt_email.Clear();
            txt_contact_no.Clear();
            txt_pass.Clear();
            txt_verify_pass.Clear();
            lbl_username.Text = "";
        }

        private void txt_age_Leave(object sender, EventArgs e)
        {
            try
            {
                int age = int.Parse(txt_age.Text);
            }
            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "AGE MUST BE A NUMBER!", "ERROR");
                txt_age.Clear();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_username.Text.Length == 0 || txt_password.Text.Length == 0)
                    MetroFramework.MetroMessageBox.Show(this, "Username / Password Cannot Be Blank");
                else if (txt_password.Text.Length < 5)
                    MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Less Than 5 Characters");
                else
                {
                    database.openConnection();
                    int returned_value = database.loginUser(txt_username.Text, txt_password.Text);
                    database.closeConnection();
                    if (returned_value == 0)
                        MetroFramework.MetroMessageBox.Show(this, "USERNAME OR PASSWORD INCORRECT!", "USERNAME OR PASSWORD INCORRECT! PLEASE TRY AGAIN!");
                    else
                    {
                        Dashboard dashboard = new Dashboard(txt_username.Text);
                        this.Hide();
                        dashboard.Show();
                        this.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_first_name.Text.Length > 0 || txt_last_name.Text.Length > 0 || txt_age.Text.Length > 0 || txt_address.Text.Length > 0 || txt_email.Text.Length > 0 || txt_contact_no.Text.Length > 0 || txt_pass.Text.Length > 0 || txt_verify_pass.Text.Length > 0)
                {
                        database.openConnection();
                        int result = database.saveUser(txt_first_name.Text, txt_last_name.Text, int.Parse(txt_age.Text), txt_address.Text, txt_email.Text, txt_contact_no.Text, txt_pass.Text, lbl_username.Text);
                        database.closeConnection();
                        if (result == 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Failed Registering User. Please Try Again!.");
                        }
                        else if (result == 1)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Successfully Saved User Info. Please Login Using Username and Password!");
                            clearandcloseregistrationpanel();
                        }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Complete All The Fields!");
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void txt_contact_no_Leave(object sender, EventArgs e)
        {
            if (txt_contact_no.Text.ToString().Any(char.IsLetter))
            {
                MetroFramework.MetroMessageBox.Show(this, "Contact Number Cannot Contain Letters!");
                txt_contact_no.Clear();
            }
            else if (txt_contact_no.Text.Length >= 10)
            {
                MetroFramework.MetroMessageBox.Show(this, "Contact Number Cannot Be More Than 10!");
                txt_contact_no.Clear();
            }
        }

        private void txt_first_name_Leave(object sender, EventArgs e)
        {
            if (txt_first_name.Text.Length > 0)
                if (txt_first_name.Text.ToString().Any(char.IsDigit) || txt_first_name.Text.ToString().Any(char.IsWhiteSpace))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please Check Name!");
                    txt_first_name.Clear();
                }
        }

        private void txt_email_Leave(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(txt_email.Text);
            if (!match.Success)
            {
                MetroFramework.MetroMessageBox.Show(this, "Enter A Valid Email!");
                txt_email.Clear();
            }
        }

        private void txt_pass_Leave(object sender, EventArgs e)
        {
            if (txt_pass.Text.Length > 0)
            {
                if (txt_pass.Text.Length > 25)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Greater Than 25 Characters!");
                    txt_pass.Clear();
                }else if(txt_pass.Text.Length <= 5)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Less Than 5 Characters!");
                    txt_pass.Clear();
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Empty!");
                txt_pass.Clear();
            }
        }

        private void txt_verify_pass_Leave(object sender, EventArgs e)
        {
            if (txt_verify_pass.Text.Length > 0)
            {
                if (txt_verify_pass.Text.Length > 25)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Greater Than 25 Characters!");
                }
                else if (txt_verify_pass.Text.Length <= 5)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Less Than 5 Characters!");
                    txt_pass.Clear();
                }
                else if (txt_verify_pass.Text != txt_pass.Text)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Passwords Do Not Match!");
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Empty!!");
            }
        }

        private void txt_username_Leave(object sender, EventArgs e)
        {
            if (txt_username.Text.Length == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Username Cannot Be Empty!!");
            }
            else if (txt_username.Text.Length > 50)
            {
                MetroFramework.MetroMessageBox.Show(this, "Username Cannot Be More Than 50 Characters!!");
            }
        }

        private void txt_password_Leave(object sender, EventArgs e)
        {
            if (txt_password.Text.Length == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Empty!!");
            }
            else if (txt_password.Text.Length < 5)
            {
                MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be Less Than 5 Characters!!");
            }
            else if (txt_password.Text.Length > 25)
            {
                MetroFramework.MetroMessageBox.Show(this, "Password Cannot Be More Than 25 Characters!!");
                txt_password.Select();
            }
        }
    }
}
