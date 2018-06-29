<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="AnalysisResults.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.AnalysisResults" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/Designer/ctrl_AnalysisResults.ascx" TagPrefix="uc1" TagName="ctrl_AnalysisResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:ctrl_AnalysisResults runat="server" id="ctrl_AnalysisResults" />
</asp:Content>
