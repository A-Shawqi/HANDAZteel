<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Anonymous.SignUp" %>

<%@ Register Src="~/UserControls/Anonymous/Ctrl_SignUp.ascx" TagPrefix="uc1" TagName="Ctrl_SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:Ctrl_SignUp runat="server" id="Ctrl_SignUp" />
</asp:Content>
