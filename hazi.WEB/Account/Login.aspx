﻿<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="hazi.WEB.Account.Login" Async="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <asp:UpdatePanel runat="server" ID="MainUpdatePanel">
        <ContentTemplate>
            <asp:PlaceHolder runat="server" ID="LoginForm" Visible="false">
                <div class="row">
                    <div class="col-md-8">
                        <section id="loginForm">
                            <div class="form-horizontal">
                                <h4>Use a local account to log in.</h4>
                                <hr />
                                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                    <p class="text-danger">
                                        <asp:Literal runat="server" ID="FailureText" />
                                    </p>
                                </asp:PlaceHolder>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">User name</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                            CssClass="text-danger" ErrorMessage="The user name field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10">
                                        <div class="checkbox">
                                            <asp:CheckBox runat="server" ID="RememberMe" />
                                            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                                    </div>
                                </div>
                            </div>
                            <p>
                                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
                                if you don't have a local account.
                            </p>
                        </section>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="SucLogin" Visible="false">
                <asp:Label ID="helloLabel" runat="server" Text="Label"></asp:Label>
                <br />
                <br />
                <asp:Button ID="buttonLogoff" runat="server" Text="Log off" OnClick="buttonLogoff_Click" CssClass="btn btn-default" />
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function Belepes() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'SikeresBelepes');
        }
        function Kilepes() {
            __doPostBack('<%= MainUpdatePanel.ClientID %>', 'SikeresKilepes');
        }
        function KilepesHeader() {
            __doPostBack('<%= Master.SiteMasterUpdatePanel.ClientID %>', 'SikeresKilepesHeader');
        }
        function BelepesHeader() {
            __doPostBack('<%= Master.SiteMasterUpdatePanel.ClientID %>', 'SikeresBelepesHeader');
        }
    </script>
</asp:Content>
