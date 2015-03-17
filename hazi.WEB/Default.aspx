<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://getbootstrap.com/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <style>
        .Torles {
            float: right;
        }
        .linkek{
            text-decoration: underline;
        }
    </style>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Bejelentések (lista)</a></li>
            <li><a href="/Pages/Naptar.aspx">Bejelentések (naptár)</a></li>
        </ul>
        <div id="tabs-1">
            <asp:PlaceHolder runat="server" ID="Bejelentesek" Visible="false">
                <div>
                    <hgroup>
                        <h2>Bejelentések</h2>
                    </hgroup>
                    <asp:GridView ID="bejelentesekLista" runat="server" AutoGenerateColumns="False"
                        ShowFooter="True" GridLines="Vertical" CellPadding="4" ItemType="hazi.DAL.UjBejelentes"
                         HeaderStyle-BackColor="DarkBlue" HeaderStyle-ForeColor="White" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField HeaderText="Műveletek">
                                <ItemTemplate>
                                    <a class="linkek" href="/Pages/Bejelento.aspx?id=<%#: Item.ID %>">Szerkesztés</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                            <asp:BoundField DataField="KezdetiDatum" HeaderText="Kezdődátum" />
                            <asp:BoundField DataField="VegeDatum" HeaderText="Végdátum" />
                            <asp:BoundField DataField="JogcimNev" HeaderText="Jogcím" />
                            <asp:BoundField DataField="UserName" HeaderText="Felhasználó" />
                            <asp:BoundField DataField="LastEdit" HeaderText="Utoljára Módosítva" />
                            <asp:TemplateField HeaderText="Törlés">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Remove" runat="server"></asp:CheckBox>
                                    <asp:DropDownList runat="server" ID="StatusDDL"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Button ID="Ujbejelentes" runat="server" Text="Új bejelentés" Font-Bold="true" OnClick="Ujbejelentes_Click" />
                <asp:Button ID="BejelentesTorles" runat="server" Text="Törlés" Font-Bold="true" OnClick="BejelentesTorles_Click" CssClass="Torles" />
            </asp:PlaceHolder>
        </div>
    </div>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#tabs").tabs({
                beforeLoad: function (event, ui) {
                    ui.jqXHR.fail(function () {
                        ui.panel.html(
                          "Nem sikerült betölteni a tartalmat");
                    });
                }
            });
        });
    </script>
</asp:Content>
