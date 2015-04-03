<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="hazi.WEB.Pages.AdminPage" EnableEventValidation="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="/Styles/AdminStyle.css" />

    <asp:HiddenField runat="server" ID="HiddenField" Value="0" />
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Szerepkörök</a></li>
            <li><a href="#tabs-2">Jogcím beállítások</a></li>
        </ul>
        <div id="tabs-1">
            <asp:UpdatePanel runat="server" ID="FelhasznalokUpdatePanel" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="Felhasznalok" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                        CellPadding="4" ItemType="hazi.WEB.Logic.Users" HeaderStyle-BackColor="DarkBlue"
                        HeaderStyle-ForeColor="White" CssClass="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Felhasználó" SortExpression="Name" />
                            <asp:BoundField DataField="RoleMegjelenes" HeaderText="Szerepkör" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="tabs-2">
            <asp:UpdatePanel runat="server" ID="UpdatePanelJogcim" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="JogcimekGV" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                        CellPadding="4" ItemType="hazi.WEB.Logic.UjJogcim" HeaderStyle-BackColor="DarkBlue"
                        HeaderStyle-ForeColor="White" CssClass="table table-bordered">
                        <EmptyDataTemplate>
                            Nem található jogcím a db-ben!
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                            <asp:BoundField DataField="Cim" HeaderText="Jogcím" />
                            <asp:TemplateField HeaderText="Új jogcím név">
                                <ItemTemplate>
                                    <asp:TextBox ID="ujJogcimNev" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inaktív">
                                <ItemTemplate>
                                    <asp:CheckBox ID="JogcimAktiv" runat="server" Checked='<%#((bool)Eval("Inaktiv")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rögzítve szín">
                                <ItemTemplate>
                                    <asp:TextBox ID="rogszin" runat="server" Text='<%# Eval("RogzitveSzin") %>' Width="100px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jóváhagyva szín">
                                <ItemTemplate>
                                    <asp:TextBox ID="jovszin" runat="server" Text='<%# Eval("JovahagySzin") %>' Width="100px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="MentesGombDiv">
            <asp:Button ID="Mentes" runat="server" Text="Mentés" Font-Bold="true" OnClick="Mentes_Click" CssClass="Mentes" />
        </div>
        <div class="ExcelReportGombDiv">
            <asp:Button ID="ExcelReport" runat="server" Text="Excel report generálás" Font-Bold="true" OnClick="ExcelReport_Click" CssClass="ExcelReport" />
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

        $("#tabs").tabs({
            activate: function (event, ui) {
                var id = $("#tabs").tabs("option", "active");
                $('#<%= HiddenField.ClientID %>').val(id);
                if (id == 0) {
                    __doPostBack('<%= FelhasznalokUpdatePanel.ClientID %>', 'Felhasznalok');
                }
                else if (id == 1) {
                    __doPostBack('<%= UpdatePanelJogcim.ClientID %>', 'Jogcim');
                }
            }
        });
    </script>
</asp:Content>
