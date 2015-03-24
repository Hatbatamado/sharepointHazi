<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BejelentesNaptar.aspx.cs" Inherits="hazi.WEB.Pages.BejelentesNaptar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView runat="server" ID="BejelentesNaptarNezetGV" AutoGenerateColumns="False" GridLines="Vertical"
                CellPadding="4" ItemType="hazi.DAL.UjBejelentes" HeaderStyle-BackColor="DarkBlue"
                HeaderStyle-ForeColor="White" CssClass="table table-bordered">
                <EmptyDataTemplate>
                    A kiválasztott elem nem található a db-ben! Vagy nincs megtekintési joga a megadott bejelentésehez! 
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="KezdetiDatum" HeaderText="Folyamat kezdete" />
                    <asp:BoundField DataField="VegeDatum" HeaderText="Folyamat vége" />
                    <asp:BoundField DataField="JogcimNev" HeaderText="Jogcím" />
                    <asp:BoundField DataField="LastEdit" HeaderText="Utolsó módosító" />
                    <asp:BoundField DataField="LastEditTime" HeaderText="Utoljára módosítva" />
                    <asp:BoundField DataField="JovaStatus" HeaderText="Státusz" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
