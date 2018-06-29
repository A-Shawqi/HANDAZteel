<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="FrameTemplate.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.FrameTemplate" %>

<%@ Register Src="~/UserControls/Designer/ctrl_FrameTemplate.ascx" TagPrefix="uc1" TagName="ctrl_FrameTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_FrameTemplate runat="server" ID="ctrl_FrameTemplate" />
</asp:Content>
