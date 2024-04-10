using System;
using System.Configuration;
using System.Data.SqlClient;

namespace JobPortals.User
{

    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;

        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(str);
                string query = "insert into Contact values(@Name,@Email,@Subject,@Message)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name.Value.Trim());
                cmd.Parameters.AddWithValue("@Email", email.Value.Trim());
                cmd.Parameters.AddWithValue("@Subject", subject.Value.Trim());
                cmd.Parameters.AddWithValue("@Message", message.Value.Trim());
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    //Response.Write("<script>alert('Message Sent Successfully')</script>");
                    lblMsg.Visible = true;
                    lblMsg.Text = "Thanks for reaching out. Will look into your query!";
                    lblMsg.CssClass = "alert alert-success";
                    clear();
                }
                else
                {
                    //Response.Write("<script>alert('Error in Sending Message')</script>");
                    lblMsg.Visible = true;
                    lblMsg.Text = "Cannot save record right now, please try again later!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error in " + ex.Message + "')</script>");

            }
            finally
            {
                con.Close();
            }
        }

        private void clear()
        {
            name.Value = string.Empty;
            email.Value = string.Empty;
            subject.Value = string.Empty;
            message.Value = string.Empty;
        }
    }
}