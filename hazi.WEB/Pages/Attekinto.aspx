<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attekinto.aspx.cs" Inherits="hazi.WEB.Pages.Attekinto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel='stylesheet' href='/Styles/AttekintoStyle.css' />
    <div runat="server" id="AttekintoUserKereso" class="AttekintoUserKereso" visible="false">
        <div class="UserLabel">
            <asp:Label runat="server" ID="AttekintoUserKeresoLabel" Text="Felhasználó:"></asp:Label>
        </div>
        <div class="UserTB">
            <asp:TextBox runat="server" ID="AttekintoUserKeresoTB"></asp:TextBox>
            <asp:Button ID="kereses" runat="server" Text="Keresés" OnClick="kereses_Click" />
        </div>
        <div class="keresoUzenet">
            <asp:Label ID="uzenet" runat="server" Text="Kis és nagy betűk számítanak a keresésnél!"></asp:Label>
        </div>
    </div>
    <div class="AttekintoTablazat">
        <asp:UpdatePanel runat="server" ID="AttekintoUpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="EvValaszto">
                    <span class="glyphicon glyphicon-arrow-left iconNyil" aria-hidden="true" onclick="DatumValtozas(0)"></span>
                    <asp:Label runat="server" ID="evLabel" Text="2015" CssClass="ev"></asp:Label>
                    <span class="glyphicon glyphicon-arrow-right iconNyil" aria-hidden="true" onclick="DatumValtozas(1)"></span>
                </div>
                <div class="Fejlec">
                    <asp:Repeater runat="server" ID="napokSzama">
                        <ItemTemplate>
                            <div class="napokSzama"><%# Eval("napokSzama") %></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <asp:Repeater runat="server" ID="KulsoRepeater">
                    <ItemTemplate>
                        <div class="HonapNeve"><%# Eval("HonapNeve") %></div>
                        <asp:Repeater runat="server" ID="BelsoRepeater" DataSource='<%# Eval("BelsoLista") %>' OnItemDataBound="BelsoRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div runat="server" id="bejelentesKocka" class="kockak">
                                    <%# Eval("JogcimNev") %>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="Jelmagyarazat">
            <div class="JelmagyarazatCim">Jelmagyarázat:</div>
            <asp:Repeater runat="server" ID="JelmagyarazatRepeater" OnItemDataBound="JelmagyarazatRepeater_ItemDataBound">
                <ItemTemplate>
                    <div runat="server" id="jelSzin" class="jelSzin"><%# Eval("BetuJel") %></div>
                    <div class="jelNev"><%# Eval("JelNev") %></div>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <script>
        function DatumValtozas(irany) {
            var ev = parseInt($('#<%= evLabel.ClientID %>').text());
            if (irany == 1) {
                ev = ev + 1;
                __doPostBack('<%= AttekintoUpdatePanel.ClientID %>', 'TextChangedJobbra');
            }
            else if (irany == 0) {
                ev = ev - 1;
                __doPostBack('<%= AttekintoUpdatePanel.ClientID %>', 'TextChangedBalra');
            }
        }
    </script>
</asp:Content>
