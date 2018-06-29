<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_AnalysisToolsM.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.Ctrl_DesignerChoice" %>
<<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="x_panel" style="height: 600px;">
        <div class="x_title">
            <h2>Choose Analysis Tools</h2>
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
                        <h2>Sap2000</h2>
                        <%--<h1>free</h1>--%>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                    <img src="../../images/Analysis%20Tools/Sap200.jpeg" style="width: 200px; height: 200px; display: block;" />
                                    <%--   <li><i class="fa fa-times text-danger"></i> 2 years access <strong> to all storage locations</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> storage</li>
                                  <li><i class="fa fa-check text-success"></i> Limited <strong> Design quota</strong></li>
                                  <li><i class="fa fa-check text-success"></i> <strong>Cash on Delivery</strong></li>
                                  <li><i class="fa fa-check text-success"></i> All time <strong> updates</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> access to all files</li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Allowed</strong> to be exclusing per sale</li>
                                </ul>--%>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <%--<a href="javascript:void(0);" class="btn btn-success btn-block" role="button">Design  </a>--%>
                            <p>
                                <%--<a href="javascript:void(0);">Sign up</a>--%>
                                <asp:Button ID="Btn_Sap2000" runat="server" CssClass="btn btn-success btn-block" Text="DownLoad File" OnClick="Btn_Sap2000_Click" />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Staad -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="pricing">
                    <div class="title">
                        <h2>Staad</h2>
                        <%--<h1>free</h1>--%>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                    <img src="../../images/Analysis%20Tools/Staad.jpg" style="width: 200px; height: 200px; display: block;" />
                                    <%--  <li><i class="fa fa-times text-danger"></i> 2 years access <strong> to all storage locations</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> storage</li>
                                  <li><i class="fa fa-check text-success"></i> Limited <strong> Design quota</strong></li>
                                  <li><i class="fa fa-check text-success"></i> <strong>Cash on Delivery</strong></li>
                                  <li><i class="fa fa-check text-success"></i> All time <strong> updates</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> access to all files</li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Allowed</strong> to be exclusing per sale</li>--%>
                                </ul>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <%--<a href="javascript:void(0);" class="btn btn-success btn-block" role="button">Design  </a>--%>
                            <p>
                                <%--<a href="javascript:void(0);">Sign up</a>--%>
                                <asp:Button ID="Btn_Staad" runat="server" CssClass="btn btn-success btn-block" Text="Download File" OnClick="Btn_Staad_Click" />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Robot -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="pricing">
                    <div class="title">
                        <h2>Robot</h2>
                        <%--<h1>free</h1>--%>
                    </div>
                    <div class="x_content">
                        <div class="">
                            <div class="pricing_features">
                                <ul class="list-unstyled text-left">
                                    <img src="../../images/Analysis%20Tools/Robot.Jpg" style="width: 200px; height: 200px; display: block;" />                                    <%-- <li><i class="fa fa-times text-danger"></i> 2 years access <strong> to all storage locations</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> storage</li>
                                  <li><i class="fa fa-check text-success"></i> Limited <strong> download quota</strong></li>
                                  <li><i class="fa fa-check text-success"></i> <strong>Cash on Delivery</strong></li>
                                  <li><i class="fa fa-check text-success"></i> All time <strong> updates</strong></li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Unlimited</strong> access to all files</li>
                                  <li><i class="fa fa-times text-danger"></i> <strong>Allowed</strong> to be exclusing per sale</li>--%>
                                </ul>
                            </div>
                        </div>
                        <div class="pricing_footer">
                            <%--<a href="javascript:void(0);" class="btn btn-success btn-block" role="button">Design  </a>--%>
                            <p>
                                <%--<a href="javascript:void(0);">Sign up</a>--%>
                                <asp:Button ID="Btn_robot" runat="server" CssClass="btn btn-success btn-block" Text="Download File" OnClick="Btn_robot_Click" />
                            </p>
                        </div>
                    </div>
                </div>
                <div class="actionBar" style="align-content:flex-end">
                <asp:Button ID="Btn_NextDesign" runat="server" CssClass="btn btn-danger" OnClick="Btn_design_Click" Text="Next " Width="165px" />
            </div>
            </div>
             
           
