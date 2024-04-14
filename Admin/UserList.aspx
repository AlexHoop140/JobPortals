<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="JobPortals.Admin.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image: url('../Images/bg.jpg'); width: 100%; height: 720px; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
        <div class="container-fluid pt-4 pb-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
        </div>
        <h3 class="text-center">Job List/Details</h3>

        <div class="row mb-3 pt-sm-3">
            <div class="col-md-12">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center"
                    EmptyDataText="No record to display!" AutoGenerateColumns="false" AllowPaging="true" PageSize="5"
                    OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="UserId" OnRowDeleting="GridView1_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="User Name">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Email" HeaderText="Email">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Mobile" HeaderText="Mobile No.">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Country" HeaderText="Country">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:CommandField CausesValidation="false" HeaderText="Delete" ShowDeleteButton="true"
                            DeleteImageUrl="../assets/img/icon/trashIcon.png" ButtonType="Image" ItemStyle-Width="8px" ItemStyle-Height="8px" />
                    </Columns>
                    <HeaderStyle BackColor="#7200CF" ForeColor="White" />
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
