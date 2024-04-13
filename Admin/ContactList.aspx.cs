using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace JobPortals.Admin
{
    public partial class ContactList : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }

            if (!IsPostBack)
            {
                showContact();
            }
        }

        private void showContact()
        {
            string query = string.Empty;
            con = new SqlConnection(str);
            query = @"SELECT Row_Number() over(Order by (Select 1)) as [Sr.No], ContactId, Name, Email, Subject, Message from Contact";
            cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            showContact();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int contactId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("delete from Contact where ContactId=@ContactId", con);
                cmd.Parameters.AddWithValue("@ContactId", contactId);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "Contact Deleted Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    showContact();
                }
                else
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "Error in Deleting Contact!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                con.Close();
                GridView1.EditIndex = -1;
                showContact();
            }
            catch (Exception ex)
            {
                con.Close();
                Response.Write("<script>alert('Error in " + ex.Message + "')</script>");
            }
        }
    }
}