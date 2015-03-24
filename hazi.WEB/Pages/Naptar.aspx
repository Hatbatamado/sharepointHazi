<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Naptar.aspx.cs" Inherits="hazi.WEB.Pages.Naptar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel='stylesheet' href='/Scripts/fullcalendar.min.css' />
</head>
<body>
    <form id="form1" runat="server">
        <div id="calendar"></div>
        <div id="dialog"></div>
    </form>
</body>
<script src='/Scripts/moment.min.js'></script>
<script src='/Scripts/fullcalendar.min.js'></script>
<script src="//code.jquery.com/jquery-1.10.2.js"></script>
<script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
<script>
    $('#calendar').fullCalendar({
        events: function (start, end, timezone, callback) {
            $.ajax({
                type: "POST",
                url: "Services.asmx/GetEventsBetween",
                data: "{ start: '" + start.toString() + "', end: '" + end.toString() + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var events = [];
                    //json foreach
                    processData(data, events);
                    callback(events);
                }
            });
        },
        eventClick: function (calEvent, jsEvent, view) {
            var dUrl = "/Pages/BejelentesNaptar.aspx?id=" + calEvent.id;
            $("#dialog").load(dUrl).dialog({
                modal: true,
                width: 925,
                height: 250,
            });
        }
    });

    function processData(data, events) {
        $.each(JSON.parse(data.d), function () {
            events.push({
                title: this.title,
                start: this.start,
                end: this.end,
                color: this.color,
                id: this.id
            });
        });
    }
</script>
</html>
