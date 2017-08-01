<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForbiddenWord.aspx.cs"
    Inherits="ProgressBar.ForbiddenWord" %>
<%@ Import Namespace="CommomCore.dll" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        //        function f1() {
        //            var n = 999;
        //            nAdd = function () {
        //                n += 1;
        //            }
        //            function f2() {
        //                alert(n);
        //            }
        //            return f2;
        //        }
        //        var result = f1();
        //        result();
        //        nAdd();
        //        result();
        var name = "The Window";
        var object = {
            name: "My Object",
            getNameFunc: function () {
                return function () {
                    return this.name;
                };
            },
            getNameFuncT: function () {
                var that = this;
                return function () {
                    return that.name;
                };
            }
        };
        alert(object.getNameFunc()());  //The Window
        alert(object.getNameFuncT()()); //My Object
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Click" />
    <%  this.JQuery("1").validate().Required("user.Name", "please provide your name!!"); %>
    </form>
    <script type="text/javascript">
        <%= this.JQuery("1").validate().ToScripts("#form") %>
    </script>
</body>
</html>
