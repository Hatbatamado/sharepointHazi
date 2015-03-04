
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bejelento.aspx.cs" Inherits="hazi.Bejelento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- http://jqueryui.com/datepicker/ jqueryui eleje -->
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.3/jquery-ui.js"></script>
    <script>
        $(function () {
            //readonly miatt felhasználó nem tud hibás dátumot beállítani
            $("#<%=datepicker1.ClientID%>").datepicker().attr('readonly', 'readonly');
            $("#<%=datepicker2.ClientID%>").datepicker().attr('readonly', 'readonly');
        });
    </script>
    <!-- jqueryui vége -->
    <!-- TODO style -->
    <div style="padding-top: 20px;">
        <div style="padding-bottom: 20px;">
            <asp:Label ID="Label1" runat="server" Text="Folyamat kezdése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datepicker1" runat="server"></asp:TextBox>
        </div>
        <div style="padding-bottom: 20px;">
            <asp:Label ID="Label2" runat="server" Text="Folyamat befejezése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datepicker2" runat="server"></asp:TextBox>
        </div>
        <div style="padding-left:50px;padding-bottom:20px;">
            <asp:DropDownList ID="DropDownList1" runat="server" ItemType="hazi.Models.Jogcim"
                SelectMethod="GetJogcimek" DataTextField="Cim" Width="175px">
            </asp:DropDownList><br />
        </div>
        <asp:Button ID="save" runat="server" Text="Mentés" OnClick="save_Click" />
        <asp:Button ID="cancel" runat="server" Text="Mégse" OnClick="cancel_Click" />
    </div>
</asp:Content>
