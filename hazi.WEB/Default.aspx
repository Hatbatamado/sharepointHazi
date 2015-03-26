<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://getbootstrap.com/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <style>
        .Torles {
            float: right;
        }
        .linkek {
            text-decoration: underline;
        }
    </style>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Bejelentések (lista)</a></li>
            <li><a href="/Pages/Naptar.aspx">Bejelentések (naptár)</a></li>
            <li><a href="/Pages/Osszegzes.aspx">Összegzés (hó)</a></li>
        </ul>
        <div id="tabs-1">
            <div>
                <hgroup>
                    <h2>Bejelentések</h2>
                </hgroup>
                <div runat="server" id="SzuroDiv" visible="false">
                    <asp:UpdatePanel runat="server" ID="SzuresPanel" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="Szuro" runat="server" Text="Szűrés:"></asp:Label><br />
                            <asp:DropDownList ID="DDLSzures" runat="server"></asp:DropDownList>
                            <asp:TextBox ID="FilterBox" runat="server" onkeyup="TextChanged();"></asp:TextBox>
                            <br />
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server" ID="MainUpdatePanel" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder runat="server" ID="Bejelentesek" Visible="false">
                            <asp:GridView ID="bejelentesekLista" runat="server" AutoGenerateColumns="False"
                                GridLines="Vertical" CellPadding="4" ItemType="hazi.DAL.UjBejelentes"
                                HeaderStyle-BackColor="DarkBlue" HeaderStyle-ForeColor="White" CssClass="table table-bordered"
                                OnDataBound="bejelentesekLista_DataBound">
                                <EmptyDataTemplate>
                                    Nem található (a szűrő által megadott) bejelentés a db-ben
                                    <br />
                                     <asp:Button ID="Vissza" runat="server" Text="Vissza" Font-Bold="true" OnClick="Vissza_Click"/>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Műveletek">
                                        <ItemTemplate>
                                            <a class="linkek" href="/Pages/Bejelento.aspx?id=<%#: Item.ID %>">Szerkesztés</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                    <asp:BoundField DataField="kezdetidatum" HeaderText="Kezdeti dátum" />
                                    <asp:BoundField DataField="VegeDatum" HeaderText="Végdátum" />
                                    <asp:BoundField DataField="JogcimNev" HeaderText="Jogcím" />
                                    <asp:BoundField DataField="UserName" HeaderText="Felhasználó" />
                                    <asp:BoundField DataField="LastEdit" HeaderText="Utolsó módosító" />
                                    <asp:BoundField DataField="LastEditTime" HeaderText="Utoljára módosítva" />
                                    <asp:BoundField DataField="JovaStatus" HeaderText="Státusz" />
                                    <asp:TemplateField HeaderText="Törlés státusz">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Remove" runat="server" Visible="false"></asp:CheckBox>
                                            <asp:DropDownList runat="server" ID="StatusDDL"
                                                SelectedValue='<%# Eval("TorlesStatus") %>'
                                                DataSource='<%# Eval("TorlesStatuszList") %>'
                                                DataTextField="Text" DataValueField="Value" Visible="false" Width="150px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </asp:PlaceHolder>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="Ujbejelentes" runat="server" Text="Új bejelentés" Font-Bold="true" OnClick="Ujbejelentes_Click" />
                <asp:Button ID="BejelentesTorles" runat="server" Text="Törlés" Font-Bold="true" OnClick="BejelentesTorles_Click" CssClass="Torles" />
            </div>
        </div>
    </div>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script type="text/javascript">
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
        function TextChanged() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'FilterTextChanged');
        }
    </script>
</asp:Content>