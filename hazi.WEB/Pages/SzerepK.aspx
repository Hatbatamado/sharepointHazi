<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SzerepK.aspx.cs" Inherits="hazi.WEB.Pages.SzerepK" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .Mentes {
            float: right;
        }
    </style>
    <asp:GridView ID="Felhasznalok" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
        CellPadding="4" ItemType="hazi.WEB.Logic.Users" HeaderStyle-BackColor="DarkBlue"
        HeaderStyle-ForeColor="White" CssClass="table table-bordered" Visible="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Felhasználó" SortExpression="Name" />
            <asp:BoundField DataField="Role" HeaderText="Szerepkör" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="Szerepkor" runat="server" Text="Új szerepkör"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:DropDownList runat="server" ID="SzerepkorDDL"
                        SelectedValue='<%# Eval("Role") %>'
                        DataSource='<%# Eval("RoleList") %>'
                        DataTextField="Text" DataValueField="Value">
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button ID="Mentes" runat="server" Text="Mentés" Font-Bold="true" OnClick="Mentes_Click" CssClass="Mentes" />
</asp:Content>
