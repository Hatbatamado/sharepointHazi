<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Osszegzes.aspx.cs" Inherits="hazi.WEB.Pages.Osszegzes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:DropDownList ID="DatumValaszto" runat="server"></asp:DropDownList>--%>
        <asp:Label ID="Uzenet" runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="DarkBlue"></asp:Label>
        <br />
        <br />
        <asp:GridView ID="OsszegzesGV" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
        CellPadding="4" ItemType="hazi.DAL.UjBejelentes" HeaderStyle-BackColor="DarkBlue"
        HeaderStyle-ForeColor="White" CssClass="table table-bordered">
            <EmptyDataTemplate>
                Nincs találat az adott dátumra / nem választott dátumot!
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="JogcimNev" HeaderText="Jogcím" />
                <asp:BoundField DataField="OsszRogzitet" HeaderText="Rögzítve (óra)" />
                <asp:BoundField DataField="OsszJovahagyott" HeaderText="Jóváhagyva (óra)" />
                <asp:BoundField DataField="OsszElutasitott" HeaderText="Elutasítva (óra)" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
