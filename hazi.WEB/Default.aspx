<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://getbootstrap.com/dist/css/bootstrap.min.css">
    <section>
        <asp:PlaceHolder runat="server" ID="Bejelentesek" Visible="false">
            <div>
                <hgroup>
                    <h2>Bejelentések</h2>
                </hgroup>
                <asp:ListView ID="productList" runat="server"
                    DataKeyNames="ID" ItemType="hazi.DAL.UjBejelentes" SelectMethod="GetIdoBejelentesek">
                    <LayoutTemplate>
                        <table class="table table-striped table-bordered">
                            <thead class="bg-primary">
                                <th>Műveletek
                                </th>
                                <th>ID
                                </th>
                                <th>Kezdődátum
                                </th>
                                <th>Végdátum
                                </th>
                                <th>Jogcím
                                </th>
                                <th>Felhasználó
                                </th>
                                <th>Utoljára Módosítva
                                </th>
                                <th>Törlés
                                </th>
                            </thead>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href="/Pages/Bejelento.aspx?id=<%#: Item.ID %>">Szerkesztés
                                </a>
                            </td>
                            <td>
                                <%#:Item.ID%> 
                            </td>
                            <td>
                                <%#:Item.KezdetiDatum%>
                            </td>
                            <td>
                                <%#:Item.VegeDatum%>
                            </td>
                            <td>
                                <%#:Item.JogcimNev%>
                            </td>
                            <td>
                                <%#:Item.UserName%>
                            </td>
                            <td>
                                <%#:Item.LastEdit%>
                            </td>
                            <td>
                                <asp:CheckBox id="Remove" runat="server"></asp:CheckBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <asp:Button ID="Button1" runat="server" Text="Új bejelentés" Font-Bold="true" OnClick="Button1_Click" />
        </asp:PlaceHolder>
    </section>
</asp:Content>
