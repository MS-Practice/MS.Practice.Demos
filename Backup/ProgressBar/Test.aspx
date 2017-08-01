<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="ProgressBar.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /*body
        {
            -webkit-scrollbar:;
            -webkit-scrollbar-button:;
            -webkit-scrollbar-track:;
            -webkit-scrollbar-track-piece:;
            -webkit-scrollbar-thumb:;
            -webkit-scrollbar-corner:;
            -webkit-resizer:;
        }
        */
        body{ overflow:auto;}
        body::-webkit-scrollbar
        {
            width:10px;
            height:10px;
        }
        body::-webkit-scrollbar-button
        {
            background-color:#FF7677;
            }
        body::-webkit-scrollbar-track{background:#FF66D5;}
        body::-webkit-scrollbar-track-piece {
        background:url(http://www.lyblog.net/wp/wp-content/themes/mine/img/stripes_tiny_08.png);
    }
    body::-webkit-scrollbar-thumb{
        background:#FFA711;
        border-radius:4px;
    }
    body::-webkit-scrollbar-corner {
        background:#82AFFF;
    }
    body::-webkit-scrollbar-resizer  {
        background:#FF0BEE;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <iframe frameborder="0" height="320" width="480" src="http://localhost:14773/360dooWeb/zhcx/wap/home2/index.aspx?c=3943&p=18236&k=dAd5wXf092QCajHdJzeyZ46W5ZY4ZUd_Pfwd3qafLJ69t4we5gf98K5qe0zR8MX0cf2zP8Hz">
        </iframe>
        <div title="http://player.youku.com/player.php/sid/XMTQ0NDAyNzU2MA==">asdasdasd
        </div>
        <div title="http://player.youku.com/player.php/Type/Folder/Fid/23654673/Ob/1/sid/XMTQ0NDMzNDg5Mg==/v.swf">asdadas
        </div>
        <br /><span>TEST</span>
        <div style="height:1080px;"></div>
    </div>
    </form>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script>
        var elem = $("iframe");
        $("div").find("div").toggle(function () {
            elem.attr("src", $(this).attr("title"));
        }, function () {
            elem.attr("src", "http://localhost:14773/360dooWeb/zhcx/wap/home2/index.aspx?c=3943&p=18236&k=dAd5wXf092QCajHdJzeyZ46W5ZY4ZUd_Pfwd3qafLJ69t4we5gf98K5qe0zR8MX0cf2zP8Hz");
        });
        $("span").toggle(function () {
            elem.attr("src", "http://player.youku.com/embed/XMTQzMjI2MzAwOA==");
        }, function () {
            elem.attr("src", "http://localhost:14773/360dooWeb/zhcx/wap/home2/index.aspx?c=3943&p=18236&k=dAd5wXf092QCajHdJzeyZ46W5ZY4ZUd_Pfwd3qafLJ69t4we5gf98K5qe0zR8MX0cf2zP8Hz");
        });
        function myBrowser() {
            var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
            var isOpera = userAgent.indexOf("Opera") > -1;
            if (isOpera) {
                return "Opera"
            }; //判断是否Opera浏览器
            if (userAgent.indexOf("Firefox") > -1) {
                return "FF";
            } //判断是否Firefox浏览器
            if (userAgent.indexOf("Chrome") > -1) {
                return "Chrome";
            }
            if (userAgent.indexOf("Safari") > -1) {
                return "Safari";
            } //判断是否Safari浏览器
            if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
                return "IE";
            }; //判断是否IE浏览器
            if (userAgent.indexOf("Edge") > -1) {
                return "IE";
            }; 
        }
        alert(myBrowser());
    </script>
</body>
</html>
