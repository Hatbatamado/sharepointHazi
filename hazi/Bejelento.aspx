<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bejelento.aspx.cs" Inherits="hazi.Bejelento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- jqueryui dátum és idő http://xdsoft.net/jqplugins/datetimepicker/ -->
    <link rel="stylesheet" type="text/css" href="/Content/jquery.datetimepicker.css"/ >
    <!-- css -->
    <style>
        #fent { padding-top: 20px; }
        .alul { padding-bottom: 20px; }
        #gombBeljebb { padding-left: 40px; }
    </style>
    <!-- css vége -->
    <div id="fent">
        <div class="alul">
            <asp:Label ID="Label1" runat="server" Text="Folyamat kezdése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datetimepicker1" runat="server" CssClass="some_class"></asp:TextBox>
        </div>
        <div class="alul">
            <asp:Label ID="Label2" runat="server" Text="Folyamat befejezése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datetimepicker2" runat="server" CssClass="some_class"></asp:TextBox>
        </div>
        <div class="alul">
            <asp:Label ID="Label3" runat="server" Text="Jogcím:" Width="150px"></asp:Label>
            <asp:DropDownList ID="DropDownList1" runat="server" ItemType="hazi.Models.Jogcim"
                SelectMethod="GetJogcimek" DataTextField="Cim">
            </asp:DropDownList><br />
        </div>
        <asp:Button ID="save" runat="server" Text="Mentés" OnClick="save_Click" Font-Bold="true" />
        <span id="gombBeljebb">
            <asp:Button ID="cancel" runat="server" Text="Mégse" OnClick="cancel_Click" />
        </span>
    </div>
    <script src="/Scripts/jquery.js"></script>
    <script src="/Scripts/jquery.datetimepicker.js"></script>
    <script>
        $('.some_class').datetimepicker().attr('readonly', 'readonly');
    </script>
</asp:Content>
