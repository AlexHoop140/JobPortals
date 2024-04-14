using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobPortals.Admin
{
    public partial class UserList : System.Web.UI.Page
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
                ShowUsers();
            }
        }

        private void ShowUsers()
        {
            string query = string.Empty;
            con = new SqlConnection(str);
            query = @"SELECT Row_Number() over(Order by (Select 1)) as [Sr.No], UserId, Name, Email, Mobile, Country from [User]";
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
            ShowUsers();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int UserId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("delete from [User] where UserId=@ContactId", con);
                cmd.Parameters.AddWithValue("@ContactId", UserId);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "User Deleted Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    ShowUsers();
                }
                else
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "Error in Deleting Contact!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                GridView1.EditIndex = -1;
                ShowUsers();
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
    }
}