<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://getbootstrap.com/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link rel='stylesheet' href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/2.3.1/fullcalendar.css" />
    <link rel='stylesheet' href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/2.3.1/fullcalendar.min.css" />
    <style>
        .Torles {
            float: right;
        }

        .linkek {
            text-decoration: underline;
        }

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
    <asp:HiddenField runat="server" ID="HiddenField" Value="0" />
    <div id="tabs">
        <asp:PlaceHolder runat="server" ID="TabLinkek">
            <ul>
                <li><a href="#tabs-1">Bejelentések (lista)</a></li>
                <li><a href="/Pages/Naptar.aspx">Bejelentések (naptár)</a></li>
                <li><a href="/Pages/Osszegzes.aspx">Összegzés (hó)</a></li>
            </ul>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="Tab">
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
                                        <asp:Button ID="Vissza" runat="server" Text="Vissza" Font-Bold="true" OnClick="Vissza_Click" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Műveletek">
                                            <ItemTemplate>
                                                <a class="linkek" href="/Default.aspx?id=<%#: Item.ID %>">Szerkesztés</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                        <asp:BoundField DataField="kezdetidatum" HeaderText="Kezdeti dátum" />
                                        <asp:BoundField DataField="VegeDatum" HeaderText="Végdátum" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="jogcimLabel" runat="server" Text="Jogcím"></asp:Label>
                                                <br />
                                                <input id="jogcimFilter" type="text" runat="server" onkeyup="FilterByJogcim();" style="width: 100px; color: black;" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="jogcim" runat="server" Text='<%# Eval("JogcimNev")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="felhasznaloLabel" runat="server" Text="Felhasználó"></asp:Label>
                                                <br />
                                                <input id="usernameFilter" type="text" runat="server" onkeyup="FilterByFelhasznalo();" style="width: 100px; color: black;" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="username" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lasteditLabel" runat="server" Text="Utolsó módosító"></asp:Label>
                                                <br />
                                                <input id="lasteditFilter" type="text" runat="server" onkeyup="FilterByLastedit();" style="width: 100px; color: black;" />
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
                            </asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="Ujbejelentes" runat="server" Text="Új bejelentés" Font-Bold="true" OnClick="Ujbejelentes_Click" />
                    <asp:Button ID="BejelentesTorles" runat="server" Text="Törlés" Font-Bold="true" OnClick="BejelentesTorles_Click" CssClass="Torles" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div id="tabs-2">
            <div id="calendar"></div>
            <div id="dialog"></div>
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="BejelentoForm" Visible="false">
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
            <asp:Button ID="save" runat="server" Text="Mentés" OnClientClick="Mentes();" OnClick="save_Click" Font-Bold="true" />
            <span id="gombBeljebb">
                <asp:Button ID="cancel" runat="server" Text="Mégse" OnClick="cancel_Click" CausesValidation="false" />
            </span>
        </div>
    </asp:PlaceHolder>
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
                },
                activate: function (event, ui) {
                    var id = $("#tabs").tabs("option", "active");
                    if (id != 0) {
                        $("#fent").hide();
                    }
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
        function Mentes() {
            __doPostBack('', 'Mentes');
        }
        $(function () {
            $("#<%=datepicker.ClientID%>").datepicker({ dateFormat: 'yy.mm.dd' }).val();
        });
    </script>
</asp:Content>
