<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DesignWizard.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.DesignWizard" EnableEventValidation="false" %>
<%--this is way fuckign wrong but i disabled validation for this page--%>
<%@ Register Src="~/UserControls/Designer/Ctrl_DesignWizard.ascx" TagPrefix="uc1" TagName="Ctrl_DesignWizard" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Ctrl_DesignWizard runat="server" ID="Ctrl_DesignWizard" />
</asp:Content>
