<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HaviAttekinto.aspx.cs" Inherits="hazi.WEB.Pages.HaviAttekinto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="AttekintoTablazat">
        <asp:UpdatePanel runat="server" ID="HaviAttekintoUpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="HonapValaszto"></div>
                <asp:Repeater runat="server" ID="KulsoRepeater">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="RangLinkB" OnCommand="RangLinkB_Command" CommandArgument='<%# Eval("Nev") %>'>
                            <%# Eval("RangVezeto") %>
                        </asp:LinkButton>
                        <div id="rang"><%# Eval("RangNormal") %></div>
                        <div id="nev"><%# Eval("Nev") %></div>
                        <asp:Repeater runat="server" ID="BelsoRepeater" DataSource='<%# Eval("BelsoLista") %>'>
                            <ItemTemplate>
                                <div runat="server" id="bejelentesKocka" class="kockak"><%# Eval("JogcimNev") %></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
