<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Anonymous.Login" %>

<%@ Register Src="~/UserControls/Anonymous/Ctrl_LogIn.ascx" TagPrefix="uc1" TagName="Ctrl_LogIn" %>
<%@ Register Src="~/MasterPages/MasterPagesControls/ctrl_GeneralReferences.ascx" TagPrefix="uc1" TagName="ctrl_GeneralReferences" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Ctrl_LogIn runat="server" id="Ctrl_LogIn" />
</asp:Content>
