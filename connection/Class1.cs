using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace connection
{
    public class Connect
    {
        SqlConnection con;
        int n = 0;

        public void connect()
        {
            con = new SqlConnection(
                "Data Source = DESKTOP-EFHH4AI\\SQLEXPRESS; " +
                "Initial Catalog = dmrat; " +
                "Integrated Security = true;");
        }

        public void insertTopurchaseTable(int p_num, DateTime curr_date, double total_Amount)
        {
            con.Open();

            SqlDataAdapter adp = new SqlDataAdapter("" +
                "insert into purchaseTable " +
                "(purchase_number,purchase_date,total_amount) " +
                "values " +
                "('" + p_num + "','" + curr_date + "','" + total_Amount + "')", con);

            adp.SelectCommand.ExecuteNonQuery();

            con.Close();

        }

        public void insertTopurchaseProductTable(int p_id, String p_name, int QTY, double AMOUNT)
        {
            con.Open();

            SqlDataAdapter adp = new SqlDataAdapter("" +
                "insert into purchaseProductTable " +
                "(purchase_id,item_name,qty,amount) " +
                "values " +
                "('" + p_id + "','" + p_name + "','" + QTY + "','" + AMOUNT + "')", con);

            adp.SelectCommand.ExecuteNonQuery();

            con.Close();
        }

        public void deleteBypurchase_id(int p_id)
        {
            con.Open();

            SqlDataAdapter adp1 = new SqlDataAdapter("" +
                "delete from purchaseProductTable " +
                "where purchase_id='"+p_id+"'", con);

            adp1.SelectCommand.ExecuteNonQuery();

            SqlDataAdapter adp2 = new SqlDataAdapter("" +
                "delete from purchaseTable " +
                "where purchase_id='" + p_id + "'", con);

            adp2.SelectCommand.ExecuteNonQuery();

            con.Close();
        }

        public string generateUniqueNumber()
        {
            string lastFourDigit, s;

            int LASTfourDIGIT;

            connect();

            con.Open();

            SqlCommand cmd = new SqlCommand("spGetUniqueNumber",con);

            var num = cmd.ExecuteScalar();

            con.Close();

            if(num == null)
            {
                lastFourDigit = "0001";
                return lastFourDigit;
            }
            else
            {
                s = num.ToString();
                LASTfourDIGIT = Convert.ToInt32(s);
                LASTfourDIGIT++;

                s = LASTfourDIGIT.ToString();

                s = s.PadLeft(4, '0');

                return s;
            }

           
        }

    }
}
