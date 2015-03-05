<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bejelento.aspx.cs" Inherits="hazi.Bejelento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<!-- http://jqueryui.com/datepicker/ jqueryui eleje -->
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
    <!-- jqueryui vége -->--%>

    <!-- jqueryui dátum és idő eleje http://xdsoft.net/jqplugins/datetimepicker/ -->
    <link rel="stylesheet" type="text/css" href="~/Content/jquery.datetimepicker.css"/ >
    <script src="~/Scripts/jquery.js"></script>
    <script src="~/Scripts/jquery.datetimepicker.js"></script>
     <script>
         $("#<%=datetimepicker1.ClientID%>").datetimepicker();
    </script>
    <!-- jqueryui dátum és idő vége -->
    <!-- css -->
    <style>
        #fent { padding-top: 20px; }
        .alul { padding-bottom: 20px; }
        #gombBeljebb { padding-left: 40px; }
    </style>
    <!-- TODO style -->
    <div id="fent">
        <div class="alul">
            <asp:Label ID="Label1" runat="server" Text="Folyamat kezdése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datetimepicker1" runat="server" CssClass="some_class"></asp:TextBox>
        </div>
        <div class="alul">
            <asp:Label ID="Label2" runat="server" Text="Folyamat befejezése:" Width="150px"></asp:Label>
            <asp:TextBox ID="datetimepicker2" runat="server"></asp:TextBox>
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
</asp:Content>
