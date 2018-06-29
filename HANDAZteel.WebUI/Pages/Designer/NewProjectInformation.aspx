<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true"  CodeBehind="NewProjectInformation.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.NewProjectInformation" %>
<%--EnableEventValidation="false"--%>
<%@ Register Src="~/UserControls/Designer/ctrl_NewProject.ascx" TagPrefix="uc1" TagName="ctrl_NewProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_newproject runat="server" id="ctrl_NewProject" />
</asp:Content>
