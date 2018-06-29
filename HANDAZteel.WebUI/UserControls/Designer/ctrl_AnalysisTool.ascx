<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_AnalysisTool.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_AnalysisTool" %>

<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>Framing Systems</h2>
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
            <div class="x_content">

                <div class="row">

                    <p>The available framing systems</p>

                    <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/clearSpan.jpeg" alt="image">
                                <div class="mask">
                                    <p>Clear Span Frame</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="lnk_clearSpan" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <p>Clear Span Frame</p>
                            </div>
                        </div>
                    </div>
                   
                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/MultiGable.jpeg" alt="image">
                                <div class="mask">
                                    <p>Multi Gable Frame</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="lnk_MultiGable" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <p>Multi Gable Frame</p>
                            </div>
                        </div>
                    </div>

                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/MonoSlope.jpeg" alt="image">
                                <div class="mask">
                                    <p>Mono Slope Frame</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink2" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <p>Mono Slope Frame</p>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>