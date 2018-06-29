<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_DesignWizard.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.DesignWizard" %>



<%--<script src="../../validator.js"></script>--%>
<div class="pace  pace-inactive">
    <div class="pace-progress" data-progress-text="100%" data-progress="99" style="transform: translate3d(100%, 0px, 0px);">
        <div class="pace-progress-inner"></div>
    </div>
    <div class="pace-activity"></div>
</div>

<div class="clearfix"></div>

<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>-Project Information</h2>
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
                <!-- Smart Wizard -->
                <p></p>
                <div id="wizard" class="form_wizard wizard_horizontal">
                    <ul class="wizard_steps anchor">
                        <li>
                            <a href="#step-1" class="selected" isdone="1" rel="1">
                                <span class="step_no">1</span>
                                <span class="step_descr">Step 1<br>
                                    <small>Pick Design Code</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a id="lol" href="#step-2" class="disabled" isdone="1" rel="2">
                                <span class="step_no">2</span>
                                <span class="step_descr">Step 2<br>
                                    <small>Pick Analysis Tool</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-3" class="disabled" isdone="1" rel="3">
                                <span class="step_no">3</span>
                                <span class="step_descr">Step 3<br>
                                    <small>Designer Summary</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-4" class="disabled" isdone="1" rel="4">
                                <span class="step_no">4</span>
                                <span class="step_descr">Step 4<br>
                                    <small>Bim Model</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-5" class="disabled" isdone="1" rel="5">
                                <span class="step_no">5</span>
                                <span class="step_descr">Step 5<br>
                                    <small>Dxf Files</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-6" class="disabled" isdone="1" rel="6">
                                <span class="step_no">6</span>
                                <span class="step_descr">Step 5<br>
                                    <small>Documentation</small>
                                </span>
                            </a>
                        </li>
                    </ul>
                    <div class="stepContainer">

                        <div id="step-1" class="wizard_content" style="display: block;">
                            <div class="form-horizontal form-label-left">
                                <div class="col-md-12">
                                    <!-- AISC 360-10 -->
                                    <div class="col-md-3 col-sm-6 col-xs-12" style="margin-left: 20%;">
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>AISC 360-10 </h2>
                                                <h1 style="font-size: 15px">Using Sap</h1>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/2010aiscspecification-140111004754-phpapp01-thumbnail-4.jpg" style="width: 200px; height: 250px; display: block;" />
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="Btn_Aisc" runat="server" CssClass="btn btn-success btn-block" Text="Design" Width="100%" OnClick="Btn_Aisc_Click" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Egyptian Code -->
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>Egyptian Code</h2>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/165.PNG" style="width: 200px; height: 250px; display: block;" />

                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="Btn_Egc" runat="server" CssClass="btn btn-success btn-block" Text="Design" Width="100%" OnClick="Btn_Egc_Click" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="step-2" class="wizard_content" style="display: block;">
                            <div class="form-horizontal form-label-left">
                                <div class="col-md-12">
                                    <!-- Sap -->
                                    <div class="col-md-3 col-sm-6 col-xs-12" <%--style="margin-left: 15%;"--%>>
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>SAP2000</h2>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/Sap200.jpeg" style="width: 200px; height: 200px; display: block;" />
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="Btn_SapAnalyze" runat="server" CssClass="btn btn-danger" OnClick="Btn_SapAnalyze_Click" Text="Analyze" Width="80%" />
                                                        <asp:Button ID="Btn_SAPAnalyzeDesign" runat="server" CssClass="btn btn-primary" Text="Analyze & Design" Width="80%" OnClick="Btn_SAPAnalyzeDesign_Click" />
                                                        <asp:Button ID="Btn_Sap2000_Down" runat="server" CssClass="btn btn-success btn-block" Text="Download File" Visible="False" Width="100%" OnClick="Btn_Sap2000_Down_Click" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Staad -->
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>Staad</h2>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/Staad.jpg" style="width: 200px; height: 200px; display: block;" />
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="Btn_StaadAnalyze" runat="server" CssClass="btn btn-danger" OnClick="Btn_StaadAnalyze_Click" Text="Analyze" Width="80%" />
                                                        <asp:Button ID="Btn_Staad_Down" runat="server" CssClass="btn btn-success btn-block" Text="Download File" Visible="False" Width="100%" OnClick="Btn_Staad_Down_Click" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Robot -->
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>Robot</h2>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/Robot.Jpg" style="width: 200px; height: 200px; display: block;" />
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="Btn_RobotAnalyze" runat="server" CssClass="btn btn-danger" OnClick="Btn_RobotAnalyze_Click" Text="Analyze" Width="80%" />
                                                        <asp:Button ID="Btn_robot_Down" runat="server" CssClass="btn btn-success btn-block" Text="Download File" Visible="False" Width="100%" OnClick="Btn_robot_Down_Click" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Open Source -->
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="pricing">
                                            <div class="title" style="height: 50px">
                                                <h2>HANDAZ FEM Solver</h2>
                                                <small>Soon ...</small>
                                            </div>
                                            <div class="x_content">
                                                <div class="">
                                                    <div class="pricing_features">
                                                        <ul class="list-unstyled text-left">
                                                            <img src="../../images/Analysis%20Tools/OpenSource.png" style="width: 200px; height: 200px; display: block;" />
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="pricing_footer">
                                                    <p>
                                                        <asp:Button ID="btn_OSAnalyze" runat="server" CssClass="btn btn-dark" OnClick="Btn_StaadAnalyze_Click" Text="Analyze" Width="80%" Enabled="False" />
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div id="step-3" class="wizard_content" style="display: block;">
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                <div class="col-md-12">
                                    <asp:GridView ID="Grd_SectionSummary" runat="server" CssClass="table table-striped table-bordered dataTable no-footer">
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100%" />
                                        <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:GridView ID="Grd_DesignSummary" runat="server" CssClass="table table-striped table-bordered dataTable no-footer" HeaderStyle-HorizontalAlign="Center" AlternatingRowStyle-HorizontalAlign="Center">
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>

                                    <asp:Button ID="Btn_ExportToExcel" runat="server" Text="Export To Excel" CssClass="btn btn-info" OnClick="Btn_ExportToExcel_Click" />
                                </div>
                            </div>
                        </div>
                        <div id="step-4" class="wizard_content" style="display: block;">
                            <div class="col-md-12">
                                <%--<div class="form-horizontal form-label-left">--%>
                                <asp:Button ID="Btn_DownIfc" runat="server" Text="Download Ifc" CssClass="btn btn-info" OnClick="Btn_DownIfc_Click" />
                                <asp:Button ID="Btn_GoToXbimViewer" runat="server" Text="Go To Viewer" CssClass="btn btn-danger" Width="146px" OnClick="Btn_GoToXbimViewer_Click" />
                                <%--</div>--%>
                            </div>
                        </div>
                        <div id="step-5" class="wizard_content" style="display: block;">
                            <div class="col-md-12">
                                <%--<div class="form-horizontal form-label-left">--%>
                                <asp:GridView ID="Grd_Autocad" runat="server" CssClass="table table-striped table-bordered dataTable no-footer" AutoGenerateColumns="False" OnSelectedIndexChanged="Grd_Autocad_SelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField HeaderText="AutoCad File" />
                                        <asp:ButtonField HeaderText="Download" Text="Download" />
                                    </Columns>

                                </asp:GridView>
                                <asp:Button ID="Btn_DownCad1" runat="server" Text="Colums Layout" CssClass="btn btn-danger" OnClick="Btn_DownCad1_Click" />
                                <asp:Button ID="Btn_DownCad2" runat="server" Text="Elevation" CssClass="btn btn-danger" OnClick="Btn_DownCad2_Click" />
                                <asp:Button ID="Btn_DownCad3" runat="server" Text="Bracing" CssClass="btn btn-danger" OnClick="Btn_DownCad3_Click" />
                                <%--</div>--%>
                            </div>
                        </div>
                        <div id="step-6" class="wizard_content" style="display: block;">
                            <div class="col-md-12">
                                <%--<div class="form-horizontal form-label-left">--%>
                            </div>
                            <%--<asp:Button ID="Btn_ExportToword" runat="server" Text="Export To Word" CssClass="btn btn-info" OnClick="Btn_ExportToWord" />--%>
                            <%--</div>--%>
                        </div>
                    </div>

                </div>
                <!-- End SmartWizard Content -->
            </div>
        </div>
    </div>
</div>
<%--<script>
    // initialize the validator function
    validator.message['date'] = 'not a real date';

    // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
    $('form')
      .on('blur', 'input[required], input.optional, select.required', validator.checkField)
      .on('change', 'select.required', validator.checkField)
      .on('keypress', 'input[required][pattern]', validator.keypress);

    $('.multi.required')
      .on('keyup blur', 'input', function () {
          validator.checkField.apply($(this).siblings().last()[0]);
      });

    // bind the validation to the form submit event
    //$('#send').click('submit');//.prop('disabled', true);

    $('form').submit(function (e) {
        e.preventDefault();
        var submit = true;
        // evaluate the form using generic validaing
        if (!validator.checkAll($(this))) {
            submit = false;
        }

        if (submit)
            this.submit();
        return false;
    });

    /* FOR DEMO ONLY */
    $('#vfields').change(function () {
        $('form').toggleClass('mode2');
    }).prop('checked', false);

    $('#alerts').change(function () {
        validator.defaults.alerts = (this.checked) ? false : true;
        if (this.checked)
            $('form .alert').remove();
    }).prop('checked', false);
</script>--%>
<!-- form wizard -->
<script type="text/javascript" src="/js/wizard/jquery.smartWizard.js"></script>
<!-- pace -->
<script src="/js/pace/pace.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        // Smart Wizard
        $('#wizard').smartWizard();
    });
</script>
