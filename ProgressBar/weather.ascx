<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="weather.ascx.cs" Inherits="ProgressBar.weather" %>
<asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
        时间：<%# (Container.DataItem as ProgressBar.Commom.Comment).CreateTime.ToString() %><br />
        内容：<%# (Container.DataItem as ProgressBar.Commom.Comment).Content %> 
    </ItemTemplate>
    <SeparatorTemplate>
        <hr />
    </SeparatorTemplate>
    <FooterTemplate>
        <hr />
    </FooterTemplate>
</asp:Repeater>
