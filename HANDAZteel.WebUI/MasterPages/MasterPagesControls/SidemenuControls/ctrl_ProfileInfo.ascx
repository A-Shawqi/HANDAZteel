<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_ProfileInfo.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_ProfileInfo" %>

<div class="profile">
    <div class="profile_pic">
        <img src="<%=GetImageURL() %>" alt="..." class="img-circle profile_img">
    </div>
    <div class="profile_info">
        <span>Welcome,</span>
        <h2><%=GetUserFullName() %></h2>
    </div>
</div>
