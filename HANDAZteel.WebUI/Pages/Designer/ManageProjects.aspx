<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ManageProjects.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.ManageProjects" %>

<%@ Register Src="~/UserControls/Designer/ctrl_ManageProjects.ascx" TagPrefix="uc1" TagName="ctrl_ManageProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc1:ctrl_manageprojects runat="server" id="ctrl_ManageProjects" />
</asp:Content>
