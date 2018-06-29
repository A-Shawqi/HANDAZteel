<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DesignerSummary.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.DesignerSummary" %>
<%@ Register Src="~/UserControls/Designer/Ctrl_resultsTable.ascx" TagPrefix="uc1" TagName="Ctrl_resultsTable" %>
<%--<%@ Register Src="~/UserControls/Designer/ctrl_robotStarter.ascx" TagPrefix="uc1" TagName="ctrl_robotStarter" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Ctrl_resultsTable runat="server" ID="Ctrl_resultsTable" />
    <%--<uc1:ctrl_robotStarter runat="server" ID="ctrl_robotStarter" />--%>
    </asp:Content>
