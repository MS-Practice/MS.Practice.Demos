<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Html5UrlNoReflash.aspx.cs"
    Inherits="ProgressBar.Html5UrlNoReflash" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTML5 修改浏览器url而不刷新页面</title>
    <script type="text/javascript">
        var domLoad = function () {
            if (ua != null && ua[1] < 10) {
                alert("浏览器不支持该功能");
                return;
            }
            if (location.href.indexOf("?") > -1) {
                var urlparts = location.href.match(/(.+?)\?.+/i);
                var urlbase = urlparts[1];
            } else {
                var urlbase = location.href;
            }
            var page = <%=page %>
            var ua = window.navigator.userAgent.match(/msie (\d\.\d)/i);
            var content = document.getElementById("content");
            var loading = document.getElementById("loading");
            //HTML5属性
            window.history.replaceState(
            {
                content: content.innerHTML,
                page: page
            },
            page,
            urlbase + (page > 1 ? "?page=" + page : "")
            );


            var ajax = new XMLHttpRequest();
            var ajaxCallBack = function () {
                if (ajax.readState == 4) {
                    loading.style.display = "none";
                    content.innerHTML = ajax.responseText;
                    window.history.pushState(
                    {
                        content: content.innerHTML,
                        page: page
                    },
                    page,
                    urlbase + "?page=" + page
                     );
                    next.href = urlbase + "?page=" + (page + 1);
                }
            };

            var next = document.getElementById("next");
            var nextClickEvent = function (event) {
                if (loading.style.display != "block") {
                    loading.style.display = "block";
                    page++;
                    ajax.open("GET", urlbase + "?page=" + page + "&ajaxload=on", true);
                    ajax.onreadystatechange = ajaxCallback;
                    ajax.send("");
                    event.preventDefault();
                }
            };
            next.addEventListener("click", nextClickEvent, false);

            var popstate = function () {
                content.innerHTML = history.state.content;
                page = history.state.page;
            };
            window.addEventListener("popstate", popstate, false);
        };
 
        try{
	        window.addEventListener("DOMContentLoaded", domLoaded, false);
        }
        catch(e){
	        alert("您的浏览器不支持");	
        }
    </script>
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <p id="content">
        <%="第" + page + "页的内容"%>
    </p>
    <p>
        <a id="next" href="?page=<%=page %>">下一页</a>
    </p>
    <div id="loading" style="display: none;">
        加载中
    </div>
    </form>
</body>
</html>
