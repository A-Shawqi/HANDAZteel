<%-- PLEASE RE ENABLE VALIDATION --%>
<%@ Page Title="New Project Inputs" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CustomerInputs.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.CustomerInputs" EnableEventValidation="false"%>

<%@ Register Src="~/UserControls/Designer/ctrl_CustomerInputs.ascx" TagPrefix="uc1" TagName="ctrl_CustomerInputs" %>
<%@ Register Src="~/UserControls/Designer/ctrl_ViewerPreviewReferences.ascx" TagPrefix="uc1" TagName="ctrl_ViewerPreviewReferences" %>
<%@ Register Src="~/UserControls/Designer/ctrl_ViewerPreview.ascx" TagPrefix="uc1" TagName="ctrl_ViewerPreview" %>
<%@ Register Src="~/UserControls/Designer/Ctrl_Image_view.ascx" TagPrefix="uc1" TagName="Ctrl_Image_view" %>
<%@ Register Src="~/UserControls/Designer/ctrl_InputsSketchesGuide.ascx" TagPrefix="uc1" TagName="ctrl_InputsSketchesGuide" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <uc1:ctrl_ViewerPreviewReferences runat="server" id="ctrl_ViewerPreviewReferences" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_CustomerInputs runat="server" ID="ctrl_CustomerInputs" />
    <uc1:ctrl_InputsSketchesGuide runat="server" id="ctrl_InputsSketchesGuide" />

    <uc1:ctrl_ViewerPreview runat="server" ID="ctrl_ViewerPreview" />
</asp:Content>
