<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hazi.WEB._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://getbootstrap.com/dist/css/bootstrap.min.css">
    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>
            <asp:ListView ID="productList" runat="server"
                DataKeyNames="ID" ItemType="hazi.DAL.IdoBejelentes" SelectMethod="GetIdoBejelentesek">
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
                            <%#:Item.Jogcim.Cim%>
                        </td>
                    </tr>
                    </td>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </section>
</asp:Content>
