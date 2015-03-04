
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bejelento.aspx.cs" Inherits="hazi.Bejelento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.3/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#<%=datepicker1.ClientID%>").datepicker();
            $("#<%=datepicker2.ClientID%>").datepicker();
        });
    </script>

    <asp:Label ID="Label1" runat="server" Text="Folyamat kezdése:"></asp:Label>
    <asp:TextBox ID="datepicker1" runat="server"></asp:TextBox>
    <asp:Label ID="Label2" runat="server" Text="Folyamat befejezése:"></asp:Label>
    <asp:TextBox ID="datepicker2" runat="server"></asp:TextBox>
    <asp:DropDownList ID="DropDownList1" runat="server" ItemType="hazi.Models.Jogcim" SelectMethod="GetJogcimek" DataTextField="Cim"></asp:DropDownList>
</asp:Content>
