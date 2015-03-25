<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bejelento.aspx.cs" Inherits="hazi.WEB.Pages.Bejelento" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="BejelentoForm" Visible="false">
        <!-- http://jqueryui.com/datepicker/ -->
        <link rel="stylesheet" href="//code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css">
        <!-- css -->
        <style>
            #fent {
                padding-top: 20px;
            }
            .alul {
                padding-bottom: 20px;
            }
            #gombBeljebb {
                padding-left: 40px;
            }
        </style>
        <!-- css vége -->
        <div id="fent">
            <div class="alul">
                <asp:Label ID="Label4" runat="server" Text="Bejelentő űrlap" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
            </div>
            <div class="alul">
                <asp:Label ID="hibaLabel" runat="server" Text="Label" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
                <asp:Label ID="mentesLabel" runat="server" Text="Label" Visible="false" ForeColor="Green" Font-Bold="true"></asp:Label>
            </div>
            <div class="alul">
                <asp:Label ID="Label5" runat="server" Text="Dátum:" Width="150px"></asp:Label>
                <asp:TextBox ID="datepicker" runat="server" Width="100px"></asp:TextBox>
                <asp:CustomValidator EnableClientScript="false" runat="server" ID="CustomValidatorDatum" OnServerValidate="CustomValidatorDatum_ServerValidate"></asp:CustomValidator>
            </div>
            <div class="alul">
                <asp:Label ID="Label1" runat="server" Text="Folyamat kezdése:" Width="150px"></asp:Label>
                <asp:DropDownList ID="ora1" runat="server" Width="50px"></asp:DropDownList>
                <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                <asp:DropDownList ID="perc1" runat="server" Width="50px"></asp:DropDownList>
                <asp:CustomValidator EnableClientScript="false" runat="server" ID="CustomValidatorIdopont1" OnServerValidate="CustomValidatorIdopont1_ServerValidate"></asp:CustomValidator>
            </div>
            <div class="alul">
                <asp:Label ID="Label2" runat="server" Text="Folyamat befejezése:" Width="150px"></asp:Label>
                <asp:DropDownList ID="ora2" runat="server" Width="50px"></asp:DropDownList>
                <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                <asp:DropDownList ID="perc2" runat="server" Width="50px"></asp:DropDownList>
                <asp:CustomValidator EnableClientScript="false" runat="server" ID="CustomValidatorIdopont2" OnServerValidate="CustomValidatorIdopont2_ServerValidate"></asp:CustomValidator>
            </div>
            <div class="alul">
                <asp:Label ID="Label3" runat="server" Text="Jogcím:" Width="150px"></asp:Label>
                <asp:DropDownList ID="DropDownList1" runat="server" DataValueField="ID"></asp:DropDownList><br />
            </div>
            <asp:Button ID="save" runat="server" Text="Mentés" OnClick="save_Click" Font-Bold="true" />
            <span id="gombBeljebb">
                <asp:Button ID="cancel" runat="server" Text="Mégse" OnClick="cancel_Click" CausesValidation="false" />
            </span>
        </div>
        <script src="//code.jquery.com/jquery-1.10.2.js"></script>
        <script src="//code.jquery.com/ui/1.11.3/jquery-ui.js"></script>
        <script>
            $(function () {
                $("#<%=datepicker.ClientID%>").datepicker({ dateFormat: 'yy.mm.dd' }).val();
            });
        </script>
    </asp:PlaceHolder>
</asp:Content>