using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobPortals.Admin
{
    public partial class JobList : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }

            if(!IsPostBack)
            {
                showJob();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            showJob();
        }

        private void showJob()
        {
            string query = string.Empty;
            con = new SqlConnection(str);
            query = @"SELECT Row_Number() over(Order by (Select 1)) as [Sr.No], JobId, Title, NoOfPost, Qualification, Experience,
                      LastDateToApply, CompanyName, Country, State, CreateDate from Jobs";
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
            showJob();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int jobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("delete from Jobs where JobId=@JobId", con);
                cmd.Parameters.AddWithValue("@JobId", jobId);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "Job Deleted Successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    showJob();
                }
                else
                {
                    //lblMsg.Visible = true;
                    lblMsg.Text = "Error in Deleting Job!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                con.Close();
                GridView1.EditIndex = -1;
                showJob();
            }
            catch(Exception ex)
            {
                con.Close();
                Response.Write("<script>alert('Error in " + ex.Message + "')</script>");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "EditJob")
            {
                Response.Redirect("NewJob.aspx?id=" + e.CommandArgument.ToString());
            }   
        }
    }
}