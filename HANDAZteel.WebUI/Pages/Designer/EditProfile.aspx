<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.EditProfile" %>

<%@ Register Src="~/UserControls/Designer/ctrl_EditProfile.ascx" TagPrefix="uc1" TagName="ctrl_EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_EditProfile runat="server" ID="ctrl_EditProfile" />
</asp:Content>
