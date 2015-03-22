<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" %>
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
        </ul>
        <div id="tabs-1">
            <div>
                <hgroup>
                    <h2>Bejelentések</h2>
                </hgroup>
                <asp:UpdatePanel runat="server" ID="MainUpdatePanel" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder runat="server" ID="Bejelentesek" Visible="false">
                            <asp:GridView ID="bejelentesekLista" runat="server" AutoGenerateColumns="False"
                                GridLines="Vertical" CellPadding="4" ItemType="hazi.DAL.UjBejelentes"
                                HeaderStyle-BackColor="DarkBlue" HeaderStyle-ForeColor="White" CssClass="table table-bordered">
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
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="jogcimLabel" runat="server" Text="Jogcím"></asp:Label>
                                            <br />
                                            <input id="jogcimFilter" type="text" runat="server" onkeyup="FilterByJogcim();" style="width: 100px; color:black;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="jogcim" runat="server" Text='<%# Eval("JogcimNev")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="felhasznaloLabel" runat="server" Text="Felhasználó"></asp:Label>
                                            <br />
                                           <input id="usernameFilter" type="text" runat="server" onkeyup="FilterByFelhasznalo();" style="width: 100px; color:black;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="username" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lasteditLabel" runat="server" Text="Utolsó módosító"></asp:Label>
                                            <br />
                                            <input id="lasteditFilter" type="text" runat="server" onkeyup="FilterByLastedit();" style="width: 100px; color:black;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lastedit" runat="server" Text='<%# Eval("LastEdit")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LastEditTime" HeaderText="Utoljára módosítva" />
                                    <asp:BoundField DataField="JovaStatus" HeaderText="Státusz" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="TorlesLabel" runat="server" Text="Törlés"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDLTorles" runat="server" ForeColor="Black" onchange="FilterByTorlesStatus();" Width="100px"></asp:DropDownList>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Remove" runat="server" Visible="false"></asp:CheckBox>
                                            <asp:DropDownList runat="server" ID="StatusDDL"
                                                SelectedValue='<%# Eval("TorlesStatus") %>'
                                                DataSource='<%# Eval("TorlesStatuszList") %>'
                                                DataTextField="Text" DataValueField="Value" Visible="false" Width="100px">
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
        function FilterByJogcim() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'FilterByJogcim');
        }
        function FilterByFelhasznalo() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'FilterByFelhasznalo');
        }
        function FilterByLastedit() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'FilterByLastedit');
        }
        function FilterByTorlesStatus() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'FilterByTorlesStatus');
        }
    </script>
</asp:Content>