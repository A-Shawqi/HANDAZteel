<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_SlideMenu.ascx.cs" Inherits="HANDAZ.PEB.WebUI.MasterPages.MasterPagesControls.OxygenTemplate.ctrl_SlideMenu" %>
 <!--/#home-slider-->
    <div class="main-nav">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/Pages/Anonymous/index.aspx">
                    <h1>
                        <img class="img-responsive" src="/images/HandazLogoWhite.png" style="height:40px; width:180px"  alt="logo"></h1>
                </a>
            </div>
            <div class="collapse navbar-collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li class="scroll active"><a href="/Pages/Anonymous/index.aspx">Home</a></li>
                    <li class="scroll"><a href="#team">About Us</a></li>
                    <li class="scroll"><a href="#portfolio">Templates</a></li>
                    <li class="scroll"><a href="#team">Team</a></li>
                    <%--<li class="scroll"><a href="#blog">Blog</a></li>--%>
                    <li class="scroll"><a href="#contact">Contact</a></li>
                    <li class="scroll"><a href="/Pages/Anonymous/SignUp.aspx">Register</a></li>
                    <li class="scroll"><a href="/Pages/Anonymous/LogIn.aspx">Log In</a></li>

                </ul>
            </div>
        </div>
    </div>
   <%-- /#main-nav--%>