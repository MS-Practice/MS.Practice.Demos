<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl.ascx.cs" Inherits="ProgressBar.WebUserControl" %>
<%@ Register Assembly="UpdatePanelHelper" Namespace="UpdatePanelHelper" TagPrefix="cc" %>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
				<%= DateTime.Now.ToString() %>
			</ContentTemplate>
    </asp:UpdatePanel>