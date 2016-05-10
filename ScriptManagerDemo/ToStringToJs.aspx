<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToStringToJs.aspx.cs" Inherits="ScriptManagerDemo.ToStringToJs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <script type="text/javascript">
        Type.registerNamespace("Demo");

        Demo.Parent = function () { };
        Demo.Parent.prototype = {
            toString: function () {
                return Object.getTypeName(this);
            }
        }
        Demo.Parent.registerClass("Demo.Parent");
        Demo.Child = function () {
            Demo.Child.initializeBase(this);
        }
        Demo.Child.prototype = {

    }
    Demo.Child.registerClass("Demo.Child", Demo.Parent);

    alert(new Demo.Parent());
    alert(new Demo.Child());
</script>--%>
<script type="text/javascript">
    function FunctionA() {
        this.test = function () {
            alert(this + "'s test");
        }
        this.toString = function () {
            return Object.getType(this);
        }
    }
    
//    FunctionA.prototype.showme = function () {
//        alert(this + "'s showme");
    //    }
    FunctionA.prototype = {
        showme: function (){
            alert(this + "'s showme");
        }
    }

    Object.getType = function (obj) {
        if (obj.constructor === FunctionA) return "this is FunctionAAA";
        if (obj.constructor === FunctionB) return "this is FunctionBBB";
    }
    Object.extend = function (src, dest) {
        for (var key in src) {
            console.log(key);
//            dest[key] = src[key];          
        }
    }
    Object.extend(FunctionA.prototype, FunctionB.propotype);
    function FunctionB() {
        FunctionA.call(this);
    }
    var a = new FunctionA();
    var b = new FunctionB();

    alert(a);
    alert(b);
    a.test();
    b.test();
    a.showme();
    console.log(b.constructor);
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
