<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="kareemTest.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Anonymous.kareemTest" %>

<%@ Register Src="~/UserControls/Anonymous/ctrl_testKareem.ascx" TagPrefix="uc1" TagName="ctrl_testKareem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ctrl_testKareem runat="server" id="ctrl_testKareem" />
</asp:Content>
