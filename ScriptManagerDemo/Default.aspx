<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ScriptManagerDemo.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml"><head runat="server"><title></title><script type="text/javascript" src="Scripts/prototype.js"></script><script type="text/javascript">
        if (!window.Global) {
            window.Global = new Object();
        }
        Global._RequestQueue = function () {
            this._requestDelegateQueue = new Array();
            this._requestInProgress = 0;
            this._maxConcurrentRequest = 2;
        }
        Global._RequestQueue.prototype =
        {
            enqueueRequestDelegate: function (requestDelegate) {
                this._requestDelegateQueue.push(requestDelegate);
                this._request();
            },
            next: function () {
                this._requestInProgress--;
                this._request();
            },
            _request: function () {
                if (this._requestDelegateQueue.length <= 0) return;
                if (this._requstInProgress >= this._maxConcurrentRequest) return;
                this._requestInProgress++;
                var requestDelegate = this._requestDelegateQueue.shift();
                requestDelegate.call(null);
            }
        }
        Global.requestQueue = new Global._RequestQueue();
    </script><script>
        function requestWithoutQueue() {
            for (var i = 0; i < 10; i++) {
                new Ajax.Request(
                url, {
                    method: 'post',
                    onComplete: callback
                });
            }
            function callback(xmlHttpRequest) {
//                Type.prototype.resolveInheritance = function () {
//                }
            }
        }
        function requestWithQueue() {
            for (var i = 0; i < 10; i++) {
                var requestDelegate = function () {
                    new Ajax.Requst(url, {
                        method: 'post',
                        onComplete: callback,
                        onFailur: Global.RequstQueue.next,
                        onException: Global.RequestQueue.next
                    });
                }
                Global.RequestQueue.enqueueRequestDelegate(requestDelegate);
            }
            function callback() {
                Global.RequestQueue.next;
            }
        }
    </script><script>
        //        伪造js对象（window.XMLHttpRequest、ActiveXObject）
        window._progIDs = ['Msxml2.XMLHTTP', 'Microsoft.XMLHTTP'];
        if (!window.XMLHttpRequest) {
        //如果浏览器不支持XMLHttpRequest，则自定义一个
            window.XMLHttpRequest = function () {
                for (var i = 0; i < window._progIDs.length; i++) {
                    try {
                        var xmlHttp = new _originalActiveXObject(window._progIDs[i]);
                        return xmlHttp;
                    }
                    catch (ex) { }
                }
                return null;
            }
        }
        if (window.ActiveXObject) {
            //新建一个_originalActiveXObject对象代替ActiveXObject;
            window._originalActiveXObject = window.ActiveXObject;
            window.ActiveXObject = function (id) {
                id = id.toUpperCase();
                for (var i = 0; i < window._progIDs.length; i++) {
                    if (id === window._progIDs[i].toUpperCase()) {
                        return new XMLHttpRequest();
                    }
                }
                return new _originalActiveXObject(id);
            }
        }
        window._originalXMLHttpRequest = window.XMLHttpRequest;
        window.XMLHttpRequest = function () {
            this._xmlHttpRequest = new _originalXMLHttpRequest();
            this.readyState = this._xmlHttpRequest.readyState;
            this._xmlHttpRequest.onreadystatechange = this._createDelegate(this, this._internalOnReadyStateChange);
        }
        window.XMLHttpRequest.prototype = {
            open: function (method, url, async) {
                this._xmlHttpRequest.open(method, url, async);
                this.readyState = this._xmlHttpRequest.readyState;
            },
            send: function (body) {
                var requestDelegate = this._createDelegate(this, function () {
                    this._xmlHttpRequest.send(body);
                    this.readyState = this._xmlHttpRequest.readyState;
                });
                //进入等待队列
                Global.RequestQueue.enqueueRequestDelegate(requestDelegate);
            },
            //重定义ajax请求设置头部信息方法
            setRequestHeader: function (header, value) {
                this._xmlHttpRequest.setRequestHeader(header, value);
            },
            //获取ajax请求的返回的文档类型
            getResponseHeader: function (header) {
                this._xmlHttpRequest.getReponseHeader(header);
            },
            //停止请求
            abort: function () {
                this._xmlHttpRequest.abort();
            },
            _internalOnReadyStateChange: function () {
                var xmlHttpRequest = this._xmlHttpRequest;
                try {
                    this.readyState = xmlHttpRequest.readyState;
                    this.repsonseText = xmlHttpRequest.repsonseText;
                    this.responseXML = xmlHttpRequest.responseXML;
                    this.statusText = xmlHttpRequest.statusText;
                    this.status = xmlHttpRequest.status;
                }
                catch (ex) { }
                if (this.readyState === 4) {
                    //如果一个请求完成，则执行下一个请求
                    Global.requestQueue.next();
                }
                if (this.onreadystatechange) {
                    this.onreadystatechange.call(null);
                }
            },
            _createDelegate: function (instance, method) {
                return function () {
                    return method.apply(instance, arguments);
                }
            }

        }
    </script></head><body><form id="form1" runat="server">
    <div>
        <%--<asp:ScriptManager ID="ScriptManager" runat="server">
            <Scripts>
            </Scripts>
        </asp:ScriptManager>--%>
    </div>
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="Label1" runat="server"></asp:Label>
                <br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:Button ID="Button2" runat="server" Text="PostBack" 
        onclick="Button2_Click" />
        <!--控件缓存-->
<%--        <asp:ObjectDataSource></asp:ObjectDataSource>--%>
    </form>
</body>
</html>
