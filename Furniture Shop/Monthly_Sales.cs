using System;
using System.Data;

namespace Furniture_Shop
{
    public partial class Monthly_Sales : MetroFramework.Forms.MetroForm
    {
        DatabaseDLL.Database database = null;
        DataTable dt = null;
        public Monthly_Sales()
        {
            InitializeComponent();
            dt = new DataTable();
            remaining();
        }

        public void remaining()
        {
            try
            {
                dt.Clear();
                int datesInMonth = int.Parse(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString());
                database = new DatabaseDLL.Database();
                database.openConnection();
                database.getMonthlySalesAvailable("Ashen_Renon", (DateTime.Now.Year + "-" + DateTime.Now.Month + "-01").ToString(), (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + datesInMonth)).Fill(dt);
                database.closeConnection();
                dt_Monthly_Sales.DataSource = dt;
            }catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void rb_Sold_Out_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dt.Clear();
                int datesInMonth = int.Parse(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString());
                database = new DatabaseDLL.Database();
                database.openConnection();
                database.getMonthlySalesSoldOut("Ashen_Renon", (DateTime.Now.Year + "-" + DateTime.Now.Month + "-01").ToString(), (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + datesInMonth)).Fill(dt);
                database.closeConnection();
                dt_Monthly_Sales.DataSource = dt;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message);
            }
        }

        private void rb_Remaining_CheckedChanged(object sender, EventArgs e)
        {
            remaining();
        }
    }
}
