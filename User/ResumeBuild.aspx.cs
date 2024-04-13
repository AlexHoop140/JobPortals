using System;
using System.Configuration;
using System.Data.SqlClient;

namespace JobPortals.User
{
    public partial class ResumeBuild : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader sdr;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        string query;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    showUserInfo();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void showUserInfo()
        {
            try
            {
                con = new SqlConnection(str);
                string query = "Select * from [User] where UserId=@userId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", Request.QueryString["id"]);
                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        txtUserName.Text = sdr["Username"].ToString();
                        txtFullName.Text = sdr["Name"].ToString();
                        txtEmail.Text = sdr["Email"].ToString();
                        txtMobile.Text = sdr["Mobile"].ToString();
                        txtTenth.Text = sdr["TenthGrade"].ToString();
                        txtTwelfth.Text = sdr["TwelfthGrade"].ToString();
                        txtGraduation.Text = sdr["GraduationGrade"].ToString();
                        txtPostGraduation.Text = sdr["PostGraduationGrade"].ToString();
                        txtPhd.Text = sdr["Phd"].ToString();
                        txtWork.Text = sdr["WorksOn"].ToString();
                        txtExperience.Text = sdr["Experience"].ToString();
                        txtAddress.Text = sdr["Address"].ToString();
                        ddlCountry.SelectedValue = sdr["Country"].ToString();
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "User not found!";
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    string concatQuery = string.Empty;
                    string filePath = string.Empty;
                    bool isValid = false;

                    con = new SqlConnection(str);
                    if (fuResume.HasFile)
                    {
                        if (isValidExtension(fuResume.FileName))
                        {
                            concatQuery = "Resume=@resume,";
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
                    con.Open();
                    query = @"Update [User] set Name=@Name,Email=@Email,Mobile=@Mobile,TenthGrade=@TenthGrade,
                            TwelfthGrade=@TwelfthGrade,GraduationGrade=@GraduationGrade,PostGraduationGrade=@PostGraduationGrade,
                            Phd=@Phd,WorksOn=@WorksOn,Experience=@Experience," + concatQuery + "Address=@Address,Country=@Country where UserId=@UserId";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenthGrade", txtTenth.Text.Trim());
                    cmd.Parameters.AddWithValue("@TwelfthGrade", txtTwelfth.Text.Trim());
                    cmd.Parameters.AddWithValue("@GraduationGrade", txtGraduation.Text.Trim());
                    cmd.Parameters.AddWithValue("@PostGraduationGrade", txtPostGraduation.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phd", txtPhd.Text.Trim());
                    cmd.Parameters.AddWithValue("@WorksOn", txtWork.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);

                    if (fuResume.HasFile)
                    {
                        if (isValidExtension(fuResume.FileName))
                        {
                            Guid obj = Guid.NewGuid();
                            filePath = "Resumes/" + obj.ToString() + fuResume.FileName;
                            fuResume.PostedFile.SaveAs(Server.MapPath("~/Resumes/") + obj.ToString() + fuResume.FileName);

                            cmd.Parameters.AddWithValue("@Resume", filePath.ToString());
                            isValid = true;
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Please select .doc, .docx, .pdf file for resume!";
                            lblMsg.CssClass = "alert alert-danger";
                        }

                    }
                    else
                    {
                        isValid = true;
                    }

                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Resume details updated successful..!";
                        lblMsg.CssClass = "alert alert-success";
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Cannot update the records, please try after sometime..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Cannot update the records, please try <b>Relogin</b>";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUserName.Text.Trim() + "</b> Username already exist, try new one!";
                    lblMsg.CssClass = "alert alert-danger";
                }
                else
                {
                    Response.Write("<script>alert('Error in " + ex.Message + "')</script>");
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

        private bool isValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExtensions = { ".doc", ".docx", ".pdf" };
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