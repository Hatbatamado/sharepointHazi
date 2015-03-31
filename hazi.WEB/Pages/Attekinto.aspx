<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attekinto.aspx.cs" Inherits="hazi.WEB.Pages.Attekinto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel='stylesheet' href='/Styles/AttekintoStyle.css' />
    <div class="AttekintoTablazat">
        <div class="EvValaszto"></div>
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
                        <div runat="server" id="bejelentesKocka" class='<%# Eval("Statusz") %>'>
                            <%# Eval("JogcimNev") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
