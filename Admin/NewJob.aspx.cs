using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobPortals.Admin
{
    public partial class NewJob : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        string query;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }
            Session["title"] = "Add New Job";
            if (!IsPostBack)
            {
                fillData();
            }
        }

        private void fillData()
        {
            if (Request.QueryString["id"] != null)
            {
                con = new SqlConnection(str);
                query = "SELECT * FROM Jobs WHERE JobId = @JobId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtJobTitle.Text = dr["Title"].ToString();
                        txtNoOfPost.Text = dr["NoOfPost"].ToString();
                        txtDescription.Text = dr["Description"].ToString();
                        txtQualification.Text = dr["Qualification"].ToString();
                        txtExperience.Text = dr["Experience"].ToString();
                        txtSpecialization.Text = dr["Specialization"].ToString();
                        txtLastDate.Text = Convert.ToDateTime(dr["LastDateToApply"]).ToString("yyyy-MM-dd");
                        txtSalary.Text = dr["Salary"].ToString();
                        ddlJobType.SelectedValue = dr["JobType"].ToString();
                        txtCompany.Text = dr["CompanyName"].ToString();
                        txtWebsite.Text = dr["Website"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        ddlCountry.SelectedValue = dr["Country"].ToString();
                        txtState.Text = dr["State"].ToString();
                        btnAdd.Text = "Update";
                        linkBack.Visible = true;
                        Session["title"] = "Edit Job";
                    }
                }
                else
                {
                    lblMsg.Text = "No job found!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                dr.Close();
                con.Close();
            }
        }

        protected void btn_AddClick(object sender, EventArgs e)
        {
            try
            {
                string type, concatQuery, imagePath = string.Empty;
                bool isValidToExecute = false;
                con = new SqlConnection(str);
                if (Request.QueryString["id"] != null)
                {
                    if (fuCompanyLogo.HasFile)
                    {
                        if (isValidExtension(fuCompanyLogo.FileName))
                        {
                            concatQuery = "CompanyImage= @CompanyImage,";
                        }
                        else
                        {
                            concatQuery = string.Empty;
                        }
                    }
                    else
                    {
                        concatQuery = string.Empty;
                    }
                    query = @"UPDATE Jobs SET Title = @Title, NoOfPost = @NoOfPost, Description = @Description, 
                         Qualification = @Qualification, Experience = @Experience, Specialization = @Specialization, LastDateToApply = @LastDateToApply,
                        Salary = @Salary, JobType = @JobType, CompanyName = @CompanyName," + concatQuery + @"Website = @Website, Email = @Email, Address = @Address, Country = @Country, State = @State WHERE JobId = @JobId";
                    type = "updated";

                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"].ToString());

                    if (fuCompanyLogo.HasFile)
                    {

                        if (isValidExtension(fuCompanyLogo.FileName))
                        {
                            Guid ojb = Guid.NewGuid();
                            imagePath = "Images/" + ojb.ToString() + fuCompanyLogo.FileName;
                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + ojb.ToString() + fuCompanyLogo.FileName);
                            cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                            isValidToExecute = true;
                        }
                        else
                        {
                            lblMsg.Text = "Please select .jgp, .jpeg, .png file for logo";
                            lblMsg.CssClass = "alert alert-danger";
                        }

                    }
                    else
                    {
                        isValidToExecute = true;
                    }
                }
                else
                {
                    query = @"INSERT INTO Jobs VALUES(@Title, @NoOfPost, @Description, 
                         @Qualification, @Experience, @Specialization,@LastDateToApply,
                        @Salary,@JobType,@CompanyName,@CompanyImage,@Website,@Email,@Address,@Country,@State,@CreateDate)";

                    type= "saved";

                    DateTime time = DateTime.Now;
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@CreateDate", time.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (fuCompanyLogo.HasFile)
                    {

                        if (isValidExtension(fuCompanyLogo.FileName))
                        {
                            Guid ojb = Guid.NewGuid();
                            imagePath = "Images/" + ojb.ToString() + fuCompanyLogo.FileName;
                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + ojb.ToString() + fuCompanyLogo.FileName);
                            cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                            isValidToExecute = true;
                        }
                        else
                        {
                            lblMsg.Text = "Please select .jgp, .jpeg, .png file for logo";
                            lblMsg.CssClass = "alert alert-danger";
                        }

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                        isValidToExecute = true;
                    }  
                }
                if (isValidToExecute)
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        lblMsg.Text = "Job " + type + " successfully!";
                        lblMsg.CssClass = "alert alert-success";
                        clear();
                    }
                    else
                    {
                        lblMsg.Text = "Cannot" + type + "the job, try again!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
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
            //clear all input fields
            txtJobTitle.Text = string.Empty;
            txtNoOfPost.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQualification.Text = string.Empty;
            txtExperience.Text = string.Empty;
            txtSpecialization.Text = string.Empty;
            txtLastDate.Text = string.Empty;
            txtSalary.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlCountry.ClearSelection();
            ddlJobType.ClearSelection();
            txtState.Text = string.Empty;
        }

        private bool isValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            for (int i = 0; i < fileExtensions.Length; i++)
            {
                if (fileName.Contains(fileExtensions[i]))
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }
    }
}