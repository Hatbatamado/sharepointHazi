<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HaviAttekinto.aspx.cs" Inherits="hazi.WEB.Pages.HaviAttekinto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="AttekintoTablazat">
        <asp:UpdatePanel runat="server" ID="HaviAttekintoUpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="HonapValaszto"></div>
                <asp:Repeater runat="server" ID="KulsoRepeater">
                    <ItemTemplate>
                        <div id="rang"></div>
                        <div id="nev"></div>
                        <asp:Repeater runat="server" ID="BelsoRepeater" DataSource='<%# Eval("BelsoLista") %>'>
                            <ItemTemplate>
                                <div runat="server" id="bejelentesKocka" class="kockak"></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
