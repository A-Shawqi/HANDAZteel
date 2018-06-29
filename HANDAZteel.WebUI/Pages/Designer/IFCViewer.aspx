<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="IFCViewer.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.IFCViewer" %>

<%@ Register Src="~/UserControls/Designer/ctrl_ViewerReferences.ascx" TagPrefix="uc1" TagName="ctrl_ViewerReferences" %>
<%@ Register Src="~/UserControls/Designer/ctrl_Viewer.ascx" TagPrefix="uc1" TagName="ctrl_Viewer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <uc1:ctrl_ViewerReferences runat="server" id="ctrl_ViewerReferences" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_Viewer runat="server" id="ctrl_Viewer" />
</asp:Content>
