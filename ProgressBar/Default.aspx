<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProgressBar.Default" %>

<%--<%@ Register Src="~/WebUserControl.ascx" TagName="WebUserControl" TagPrefix="uc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="<%=CommomCore.LoadResource.GetResources("Scripts/dynamicStyle.js") %>" type="text/javascript"></script>
    <script>
        function A(aa) {
            var a = "<img src="+aa+" />";
            var b = "<img src=\""+aa+"\" />";
            console.log(a + "  " + b);
        }
        A(45);
        A('45');

        console.log(window.screen)
        $(document).ready(function () {
            $("div").data("func", { "\"menu\"": "1", "\"nemu2\"": "2" });
            console.log($("div").data("func"))
            var d = new Date(Date.UTC(2015, 9, 10));
            console.log(d);
            alert(window.scroll());
        })
        var A = function () {
            this.message = "消息";
            option = {
                
            }
        }
        this.A.prototype = {
            show: function (message) {
                this.Alert(message);
            },
            callback: function () {
                alert("A.callback");
            },
            Alert: function (message) {
                window.alert(message);
            }
        }
        var a = new A();
        a.show(a.message);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerID" runat="server">
    </asp:ScriptManager>
    <%--    <uc1:WebUserControl id="WebUserControl1" runat="server" />--%>
    <div data-func="">
    asdasdasd
    </div>

    </form>
</body>
</html>
