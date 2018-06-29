<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_FrameTemplate.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_FrameTemplate" %>

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
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                        <asp:Button width="100%" ID="btn_ClearSpan" runat="server" Text="Clear Span Frame" CssClass="btn btn-success" OnClick="btn_ClearSpan_Click"  />
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
                                        <asp:Button width="100%" ID="btn_MultiGable" runat="server" Text="Multi Gable Frame" CssClass="btn btn-success" OnClick="btn_MultiGable_Click"  />
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
                                        <asp:Button width="100%" ID="btn_MonoSlope" runat="server" Text="Mono Slope Frame" CssClass="btn btn-success" OnClick="btn_MonoSlope_Click"  />
                            </div>
                        </div>
                    </div>

                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/MS-1.jpeg" alt="image">
                                <div class="mask">
                                    <p>Multi-Span "1" (MS-1)</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink1" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="btn_MultiSpan1" runat="server" Text="Multi-Span 1 (MS-1)" CssClass="btn btn-success" OnClick="btn_MultiSpan1_Click"  />
                            </div>
                        </div>
                    </div>

                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/MS-2.jpeg" alt="image">
                                <div class="mask">
                                    <p>Multi-Span "2" (MS-2)</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink3" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="btn_MultiSpan2" runat="server" Text="Multi-Span 2 (MS-2)" CssClass="btn btn-success" OnClick="btn_MultiSpan2_Click"  />
                            </div>
                        </div>
                    </div>

                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/MS-3.jpeg" alt="image">
                                <div class="mask">
                                    <p>Multi-Span "3" (MS-3)</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink4" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="btn_MultiSpan3" runat="server" Text="Multi-Span 3 (MS-3)" CssClass="btn btn-success" OnClick="btn_MultiSpan3_Click"  />
                            </div>
                        </div>
                    </div>

                     <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/ArchedSingle.jpeg" alt="image">
                                <div class="mask">
                                    <p>Arched-Single Span</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink5" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="Button1" runat="server" Text="Arched-Single Span" CssClass="btn btn-black" Enabled="false" />
                            </div>
                        </div>
                    </div>

                                         <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/ArchedMultiSpan.jpeg" alt="image">
                                <div class="mask">
                                    <p>Arched-Multi Span</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink6" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="Button2" runat="server" Text="Arched-Multi Span" CssClass="btn btn-black" Enabled="false" />
                            </div>
                        </div>
                    </div>

                                         <div class="col-md-55">
                        <div class="thumbnail">
                            <div class="image view view-first">
                                <img style="width: 100%; display: block;" src="/images/Frames/LeanTo.jpeg" alt="image">
                                <div class="mask">
                                    <p>Lean-To</p>
                                    <div class="tools tools-bottom">
                                        <asp:HyperLink ID="HyperLink7" runat="server"><i class="fa fa-pencil"></i></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="caption">
                                <asp:Button width="100%" ID="Button3" runat="server" Text="Lean-To" CssClass="btn btn-black" Enabled="false" />
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</div>