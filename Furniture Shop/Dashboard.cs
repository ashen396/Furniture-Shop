using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Furniture_Shop
{
    public partial class Dashboard : MetroFramework.Forms.MetroForm
    {
        string username, item_name;
        public Dashboard(string username)
        {
            InitializeComponent();
            this.username = username;
            lbl_hello_username.Text = "Hello " + username;
            updateListView("*");
            cmb_Search_Selector.SelectedIndex = 0;
        }

        public void updateListView(String searchtext)
        {
            try
            {
                DatabaseDLL.Database database = new DatabaseDLL.Database();
                database.openConnection();
                var items = database.getData(username);
                database.closeConnection();
                ImageList imageList = new ImageList();
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                Size size = new Size(128, 128);
                imageList.ImageSize = size;
                lv_items.LargeImageList = imageList;
                lv_items.Clear();

                for (int count = 0; count < items.Item1.Count; count++)
                {
                    if (File.Exists("./images/" + items.Item2[count]))
                    {
                        imageList.Images.Add(Image.FromFile("./images/" + items.Item2[count]));
                    }
                    else
                    {
                        imageList.Images.Add(Image.FromFile("./images/image-not-found-300x169.jpg"));
                    }
                    lv_items.Items.Add(items.Item1[count], count);
                }
            }
            catch(FileNotFoundException ex) {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        public void updateListView(string searchtype, string searchtext)
        {
            try
            {
                DatabaseDLL.Database database = new DatabaseDLL.Database();
                database.openConnection();
                var items = database.getData(username, searchtype, searchtext);
                database.closeConnection();
                ImageList imageList = new ImageList();
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                Size size = new Size(128, 128);
                imageList.ImageSize = size;
                lv_items.LargeImageList = imageList;

                for (int count = 0; count < items.Item1.Count; count++)
                {
                    if (File.Exists("./images/" + items.Item2[count]))
                    {
                        imageList.Images.Add(Image.FromFile("./images/" + items.Item2[count]));
                    }
                    else
                    {
                        imageList.Images.Add(Image.FromFile("./images/image-not-found-300x169.jpg"));
                    }
                    lv_items.Items.Add(items.Item1[count], count);
                }
            }
            catch(FileNotFoundException ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Application.Exit();
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void lbl_add_items_Click(object sender, EventArgs e)
        {
            try
            {
                Add_Items add_Items = new Add_Items(username);
                add_Items.ShowDialog();
            }catch(FileLoadException ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
            
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void lbl_add_supplier_Click(object sender, EventArgs e)
        {
            Add_Supplier add_Supplier = new Add_Supplier(username);
            add_Supplier.ShowDialog();
        }

        private void lbl_Add_Category_Click(object sender, EventArgs e)
        {
            Add_Category add_Category = new Add_Category(username);
            add_Category.ShowDialog();
        }

        private void lv_items_ItemActivate(object sender, EventArgs e)
        {
            DetailsPage detailsPage = new DetailsPage(username, item_name);
            detailsPage.ShowDialog();
        }

        private void cmb_Search_Selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Search_Selector.SelectedIndex == 0)
                {
                    txt_Search_Items.Enabled = false;
                    txt_Search_Items.Clear();
                    updateListView("*");
                }
                else if (cmb_Search_Selector.SelectedIndex == 1)
                {
                    txt_Search_Items.Enabled = true;
                }
                else if (cmb_Search_Selector.SelectedIndex == 2)
                {
                    txt_Search_Items.Enabled = true;
                }
                else if (cmb_Search_Selector.SelectedIndex == 3)
                {
                    txt_Search_Items.Enabled = true;
                }
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void txt_Search_Items_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Search_Selector.SelectedIndex == 1)
                {
                    lv_items.Clear();
                    updateListView("CodeNo", txt_Search_Items.Text);
                }
                else if (cmb_Search_Selector.SelectedIndex == 2)
                {
                    lv_items.Clear();
                    updateListView("Categoryname", txt_Search_Items.Text);
                }
                else if (cmb_Search_Selector.SelectedIndex == 3)
                {
                    lv_items.Clear();
                    updateListView("Supplier_Name", txt_Search_Items.Text);
                }
            }catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void lbl_Settings_Click(object sender, EventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage(username);
            settingsPage.ShowDialog();
        }

        private void lbl_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Search_Selector.SelectedIndex == 0)
                {
                    lv_items.Clear();
                    updateListView("*");
                }
                else if (cmb_Search_Selector.SelectedIndex == 1)
                {
                    lv_items.Clear();
                    updateListView("CodeNo", txt_Search_Items.Text);
                }
                else if (cmb_Search_Selector.SelectedIndex == 2)
                {
                    lv_items.Clear();
                    updateListView("Categoryname", txt_Search_Items.Text);
                }
                else if (cmb_Search_Selector.SelectedIndex == 3)
                {
                    lv_items.Clear();
                    updateListView("Supplier_Name", txt_Search_Items.Text);
                }
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void lv_items_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            item_name = e.Item.Text;
        }
    }
}
