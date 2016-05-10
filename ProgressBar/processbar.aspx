<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="processbar.aspx.cs" Inherits="ProgressBar.processbar" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function load() {
            document.getElementById("bar").style.width = "0px";
            var scripts = new Array();
            for (var i = 0; i < 8; i++) {
                var s = new Object();
                var sleep = Math.round((Math.random() * 400)) + 100;
                s.url = "Script.ashx?sleep=" + sleep + "&t=" + Math.random();
                s.cost = sleep;
                scripts.push(s);
            }
            Jeffz.Sample.LoadScripts.load(scripts);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager2" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/processBarLoad.js" />
            </Scripts>
        </asp:ScriptManager>
        Progress Bar:
        <div style="border: solid 1px black;">
            <div id="bar" style="height: 20px; width: 0%; background-color: Red;">
            </div>
        </div>
        <input type="button" onclick="Load()" value="Load" />
        <div id="message">
        </div>
    </div>
    </form>
</body>
</html>
