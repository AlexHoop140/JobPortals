using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobPortals.User
{
    public partial class JobDetails : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt, dt1;
        string str = System.Configuration.ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        public string jobTitle = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                showJobDetail();
                DataBind();
            }
            else
            {
                Response.Redirect("JobList.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void showJobDetail()
        {

            con = new SqlConnection(str);
            //string query = @"select row_number() over(order by (select 1)) as [sr.no], jobid, title, noofpost, qualification, experience,
            //      lastdatetoapply, companyname, country, state, createdate, imageurl from jobs";
            string query = @"SELECT * FROM Jobs WHERE JobId=@id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            DataList1.DataSource = dt;
            DataList1.DataBind();
            jobTitle = dt.Rows[0]["Title"].ToString();
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ApplyJob")
            {
                if (Session["user"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    try
                    {
                        con = new SqlConnection(str);
                        string query = "insert into AppliedJobs values(@JobId, @UserId, @AppliedDate)";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
                        cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                        cmd.Parameters.AddWithValue("@AppliedDate", DateTime.Now);
                        con.Open();
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Job Applied Successfully!";
                            lblMsg.CssClass = "alert alert-success";
                            showJobDetail();
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Error in Applying Job!";
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
            }
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (Session["user"] != null)
            {
                LinkButton btnApplyJob = e.Item.FindControl("lbApplyJob") as LinkButton;
                if(isApplied())
                {
                    btnApplyJob.Enabled = false;
                    btnApplyJob.Text = "Applied";
                }
                else
                {
                    btnApplyJob.Enabled = true;
                    btnApplyJob.Text = "Apply Now";
                }
            }
        }

        bool isApplied()
        {
            con = new SqlConnection(str);
            string query = @"SELECT * FROM AppliedJobs WHERE (UserId=@UserId) AND JobId=@JobId";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
            sda = new SqlDataAdapter(cmd);
            dt1 = new DataTable();
            sda.Fill(dt1);
            if(dt1.Rows.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GetImageUrl(Object url)
        {
            string url1 = string.Empty;
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "~/Images/No_image.png";
            }
            else
            {
                url1 = string.Format("~/{0}", url);
            }

            return ResolveUrl(url1);
        }
    }
}