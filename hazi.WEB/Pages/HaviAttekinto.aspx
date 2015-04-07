<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HaviAttekinto.aspx.cs" Inherits="hazi.WEB.Pages.HaviAttekinto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel='stylesheet' href='/Styles/HaviAttekintoStyle.css' />
    <div class="AttekintoTablazat">
        <asp:UpdatePanel runat="server" ID="HaviAttekintoUpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="HonapValaszto">
                    <span class="glyphicon glyphicon-arrow-left iconNyil" aria-hidden="true" onclick="DatumValtozas(0)"></span>
                    <asp:Label runat="server" ID="honapLabel" Text="" CssClass="honap"></asp:Label>
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
                        <div class="Felhasznalok">
                            <asp:LinkButton runat="server" ID="RangLinkB" OnCommand="RangLinkB_Command" CommandArgument='<%# Eval("Nev") %>'
                                 CommandName='<%# Eval("RangVezeto") %>' CssClass="lenyilo">
                            <%# Eval("RangVezeto") %>
                            </asp:LinkButton>
                            <div id="rang" class="rang"><%# Eval("RangNormal") %></div>
                            <div id="nev" class="nev"><%# Eval("Nev") %></div>
                        </div>
                        <asp:Repeater runat="server" ID="BelsoRepeater" DataSource='<%# Eval("BelsoLista") %>' OnItemDataBound="BelsoRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div runat="server" id="bejelentesKocka" class="kockak"><%# Eval("JogcimNev") %></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        function DatumValtozas(irany) {
            var ev = parseInt($('#<%= honapLabel.ClientID %>').text());
            if (irany == 1) {
                ev = ev + 1;
                __doPostBack('<%= HaviAttekintoUpdatePanel.ClientID %>', 'TextChangedJobbra');
            }
            else if (irany == 0) {
                ev = ev - 1;
                __doPostBack('<%= HaviAttekintoUpdatePanel.ClientID %>', 'TextChangedBalra');
            }
        }
    </script>
</asp:Content>
