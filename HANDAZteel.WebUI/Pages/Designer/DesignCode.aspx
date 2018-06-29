<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DesignCode.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.DesignCode" %>

<%@ Register Src="~/UserControls/Designer/Ctrl_DesignCode.ascx" TagPrefix="uc1" TagName="Ctrl_DesignCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Ctrl_DesignCode runat="server" id="Ctrl_DesignCode" />
</asp:Content>
