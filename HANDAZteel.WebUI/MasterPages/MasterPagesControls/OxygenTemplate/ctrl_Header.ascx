<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_Header.ascx.cs" Inherits="HANDAZ.PEB.WebUI.MasterPages.MasterPagesControls.OxygenTemplate.Header" %>
<header id="home">
    <div id="home-slider" class="carousel slide carousel-fade" data-ride="carousel">
        <div class="carousel-inner">
            <div class="item active" style="height: 641px; background-image: url(&quot;/images/slider/1.jpg&quot;);">
                <div class="caption">
                    <h1 class="animated fadeInLeftBig">Welcome to <span><%= Resources.WebResources.siteTitle%></span></h1>
                    <p class="animated fadeInRightBig">Design - View - Manage - Collaborate</p>
                  
                      <a data-scroll="" class="btn btn-start animated fadeInUpBig" href="#portfolio">Start now</a>
                </div>
            </div>
            <div class="item" style="height: 641px; background-image: url(&quot;/images/slider/2.jpg&quot;);">
                <div class="caption">
                    <h1 class="animated fadeInLeftBig">Say Hello to <span><%= Resources.WebResources.siteTitle%></span></h1>
                    <p class="animated fadeInRightBig">Design - View - Manage - Collaborate</p>
                    <a data-scroll="" class="btn btn-start animated fadeInUpBig" href="#portfolio">Start now</a>
                </div>
            </div>
            <div class="item" style="height: 641px; background-image: url(&quot;/images/slider/3.jpg&quot;);">
                <div class="caption">
                    <h1 class="animated fadeInLeftBig">We are <span><%= Resources.WebResources.siteTitle%></span></h1>
                    <p class="animated fadeInRightBig">Design - View - Manage - Collaborate</p>
                    <a data-scroll="" class="btn btn-start animated fadeInUpBig" href="../../../Pages/Designer/NewProjectInformation.aspx">Start now</a>
                </div>
            </div>
        </div>
        <a class="left-control" href="#home-slider" data-slide="prev"><i class="fa fa-angle-left"></i></a>
        <a class="right-control" href="#home-slider" data-slide="next"><i class="fa fa-angle-right"></i></a>

        <a id="tohash" href="#portfolio"><i class="fa fa-angle-down"></i></a>

    </div>

     
<%--   Khaled & mark,I moved the slider to a separate user control to use it in other pages (A.Shawky)--%>
</header>
