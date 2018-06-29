<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="AnalysisTool.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.DesignerChoice" %>

<%@ Register Src="~/UserControls/Designer/Ctrl_AnalysisToolsM.ascx" TagPrefix="uc1" TagName="Ctrl_AnalysisToolsM" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Ctrl_AnalysisToolsM runat="server" ID="Ctrl_AnalysisToolsM" />
</asp:Content>
