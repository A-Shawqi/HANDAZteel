<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_LogIn.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Anonymous.Ctrl_LogIn" %>
<%@ Register Src="~/MasterPages/MasterPagesControls/ctrl_Footer.ascx" TagPrefix="uc1" TagName="ctrl_Footer" %>






<div class="col-md-6 col-xs-12">
    <div class="x_panel">
        <div class="x_title">
            <h2>Log In <small>@HANDAZ.com</small></h2>
            <ul class="nav navbar-right panel_toolbox">
                <li>
                    <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                    <ul class="dropdown-menu" role="menu">
                        <li>
                            <a href="../../Pages/Anonymous/SignUp.aspx">Register</a>
                        </li>
                        <li>
                            <a href="#">Log In</a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a class="close-link"><i <%--class="fa fa-close"--%>></i></a>
                </li>
            </ul>
            <div class="clearfix"></div>
        </div>
        <div class="x_content">
            <br />
            <div class="form-horizontal form-label-left">



                <asp:Login ID="Login1" runat="server" OnLoggedIn="Login1_LoggedIn">

                    <LayoutTemplate>

                        <div class="form-group">
                            <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User-Name</asp:Label>
                            <div class="col-md-9 col-sm-9 col-xs-12">
                                <asp:TextBox CssClass="form-control" ID="UserName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                            <div class="col-md-9 col-sm-9 col-xs-12">
                                <asp:TextBox CssClass="form-control" ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="radio">
                                <label>
                                    <asp:CheckBox CssClass="js-switch" ID="RememberMe" runat="server" Text="Remember me next time." />
                                </label>
                            </div>
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </div>
                        </div>

                         <div class="ln_solid"></div>
                        <div class="form-group">
                            <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btn_Cancel_Click" />
                                <asp:Button CssClass="btn btn-success" ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="ctl00$Login1" />

                            </div>
                        </div>

                    </LayoutTemplate>
                </asp:Login>



            </div>
        </div>
    </div>
</div>

