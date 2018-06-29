<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AnonymousUsers.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Anonymous.Index" %>

<%@ Register Src="~/UserControls/Anonymous/ctrl_Team.ascx" TagPrefix="uc1" TagName="ctrl_Team" %>
<%@ Register Src="~/UserControls/Anonymous/ctrl_Contact.ascx" TagPrefix="uc1" TagName="ctrl_Contact" %>
<%@ Register Src="~/MasterPages/MasterPagesControls/OxygenTemplate/ctrl_Header.ascx" TagPrefix="uc1" TagName="ctrl_Header" %>
<%@ Register Src="~/UserControls/Anonymous/ctrl_TemplatePictures.ascx" TagPrefix="uc1" TagName="ctrl_TemplatePictures" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<uc1:ctrl_Header runat="server" ID="ctrl_Header" />
    <uc1:ctrl_TemplatePictures runat="server" id="ctrl_TemplatePictures" />

    <uc1:ctrl_Team runat="server" ID="ctrl_Team" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_Contact runat="server" ID="ctrl_Contact" />
</asp:Content>
