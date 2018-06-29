<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.Profile" %>

<%@ Register Src="~/UserControls/Designer/ctrl_Profile.ascx" TagPrefix="uc1" TagName="ctrl_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_Profile runat="server" ID="ctrl_Profile" />

</asp:Content>
