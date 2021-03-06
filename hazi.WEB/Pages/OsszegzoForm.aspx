﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OsszegzoForm.aspx.cs" Inherits="hazi.WEB.Pages.OsszegzoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel='stylesheet' href='/Styles/OsszegzoFormStyle.css' />
    <asp:UpdatePanel runat="server" ID="JovahagyUpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="Filter" runat="server" Text="Szűrés:"></asp:Label>
            <asp:DropDownList ID="JovahagySzures" runat="server" AutoPostBack="true" OnSelectedIndexChanged="JovahagySzures_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <br />
            <asp:GridView ID="Jovahagyas" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                CellPadding="4" ItemType="hazi.DAL.UjBejelentes" HeaderStyle-BackColor="DarkBlue"
                HeaderStyle-ForeColor="White" CssClass="table table-bordered" Visible="false">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="HanyadikHet" HeaderText="Hét" SortExpression="HanyadikHet" />
                    <asp:BoundField DataField="UserName" HeaderText="Felhasználó" />
                    <asp:BoundField DataField="JogcimNev" HeaderText="Jogcím" />
                    <asp:BoundField DataField="Ido" HeaderText="Idő" />
                    <asp:BoundField DataField="OsszIdo" HeaderText="Jelenlét idő / hét" />
                    <asp:BoundField DataField="JovaStatusMegjelenes" HeaderText="Státusz" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="StatuszLabel" runat="server" Text="Új státusz"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="StatuszDDL"
                                SelectedValue='<%# Eval("JovaStatus") %>'
                                DataSource='<%# Eval("JovaStatuszList") %>'
                                DataTextField="Text" DataValueField="Value">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    Nem található elbírálandó jelentés a db-ben
            <br />
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Mentes" runat="server" Text="Mentes" Font-Bold="true" CssClass="Mentes" OnClick="Mentes_Click" />
</asp:Content>
