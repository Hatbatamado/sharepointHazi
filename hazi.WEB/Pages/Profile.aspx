<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="hazi.WEB.Pages.Profile" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel='stylesheet' href='/Styles/ProfileStyle.css' />
    <div runat="server" id="contentDiv">
        <h1>
            <asp:Label runat="server" ID="UserNameLabel"></asp:Label></h1>
        <asp:Image runat="server" ID="ProfilePictureImg" /><br />
        <p>Születési dátum:</p>
        <asp:Label runat="server" ID="BirthdayLabel"></asp:Label><br />
        <p>Vezető:</p>
        <asp:Label runat="server" ID="ManagerLabel"></asp:Label>
    </div>
</asp:Content>
