<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="hazi.WEB.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link rel='stylesheet' href='/Styles/RegisterStyle.css' />
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Új felhasználó létrehozása</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Felhasználó név</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                    CssClass="text-danger" ErrorMessage="A felhasználó név mező kitöltése kötelező!" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Jelszó</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="A jelszó mező kitöltése kötelező!" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Jelszó újra</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="A jelszó újra mező kitöltése kötelező!" />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="A két jelszó nem egyezik!" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="SzuletesiLabel" CssClass="col-md-2 control-label ujLabel">Születési dátum</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="SzuletesiTB" TextMode="Date" CssClass="form-control ujControl"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="SzuletesiTB" CssClass="text-danger"
                    ErrorMessage="A születési dátum mező kitöltése kötelező!"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="VezetoLabel" CssClass="col-md-2 control-label ujLabel">Vezető</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="VezetoDDL" CssClass="form-control  ujControl"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="VezetoDDL" CssClass="text-danger"
                    ErrorMessage="Vezető kiválasztása kötelező!"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="PKepLabel" CssClass="col-md-2 control-label ujLabel">Profil kép</asp:Label>
            <div class="col-md-10">
                <asp:FileUpload runat="server" ID="PictureFileUpload" CssClass="form-control ujControl" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Regisztráció" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
