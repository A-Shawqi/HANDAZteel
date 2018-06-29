<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_DesignCode.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.Ctrl_DesignCode" %>
<div class="x_panel" style="height: 600px;">
        <div class="x_title">
            <h2>Choose Design Code</h2>
            <ul class="nav navbar-right panel_toolbox">
                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                    <ul class="dropdown-menu" role="menu">
                        <li><a href="#">Settings 1</a>
                        </li>
                        <li><a href="#">Settings 2</a>
                        </li>
                    </ul>
                </li>
                <li><a class="close-link"><i class="fa fa-close"></i></a>
                </li>
            </ul>
            <div class="clearfix"></div>
        </div>
        <div class="col-md-12">
            <!-- Sap -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="pricing">
                    <div class="title">
                        <h2>AISC 360-10</h2>
                        <h1 style="font-size:15px">Using SAP2000</h1>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                    <img src="../../images/Analysis%20Tools/2010aiscspecification-140111004754-phpapp01-thumbnail-4.jpg" style="width:200px;height:250px; display: block;"/>
                                </ul>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <asp:Button ID="Btn_DesignAmerican" runat="server" CssClass="btn btn-success btn-block" OnClick="Btn_DesignAmerican_Click" Text="Design" />
                        </div>
                    </div>
                </div>
            </div>
            <%--  <!-- EgyptianCode -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="pricing">
                    <div class="title">
                        <h2>Egyptian Code</h2>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                      <img src="../../images/Analysis%20Tools/165.PNG" style="width:200px;height:200px; display: block;"/>
                                <div class="mask">
                                    <p>Clear Span Frame</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="lnk_clearSpan" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                                </ul>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <p>
                                &nbsp;</p>
                        </div>
                    </div>
                </div>
            </div>--%>
     <!-- Egyptian Code -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="pricing">
                    <div class="title">
                        <h2>Egyptian Code For Steel Constructions and Bridges (ASD)</h2>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                     <img src="../../images/Analysis%20Tools/165.PNG" style="width:200px;height:250px; display: block;"/>
                                </ul>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <asp:Button ID="Btn_DesignEcg" runat="server" CssClass="btn btn-success btn-block" OnClick="Btn_DesignEcg_Click" Text="Design" />
                        </div>
                    </div>
                </div>
            </div>
           
         