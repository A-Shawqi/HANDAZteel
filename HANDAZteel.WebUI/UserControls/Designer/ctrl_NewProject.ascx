<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_NewProject.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_NewProject" %>
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
                <h2>Project Information</h2>
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
                                    <small>Project Information</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-2" class="disabled" isdone="1" rel="2">
                                <span class="step_no">2</span>
                                <span class="step_descr">Step 2<br>
                                    <small>Owner Information</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-3" class="disabled" isdone="1" rel="3">
                                <span class="step_no">3</span>
                                <span class="step_descr">Step 3<br>
                                    <small>Designer Information</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-4" class="disabled" isdone="1" rel="4">
                                <span class="step_no">4</span>
                                <span class="step_descr">Step 4<br>
                                    <small>Contractor Information</small>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#step-5" class="disabled" isdone="1" rel="5">
                                <span class="step_no">5</span>
                                <span class="step_descr">Step 5<br>
                                    <small>Consultant Information</small>
                                </span>
                            </a>
                        </li>
                    </ul>

                    <div class="stepContainer">
                        <div id="step-1" class="wizard_content" style="display: block;">
                            <div class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Project Title<span class="required"></span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_ProjectName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Project Description<span class="required"></span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Description" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group"  >
                                    <label ID="lbl_LengthUnit" class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">Length Unit</label>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <asp:DropDownList ID="ddl_LengthUnit" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                 <div class="form-group"  >
                                    <label ID="lbl_ForceUnit" class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">Force Unit</label>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <asp:DropDownList ID="ddl_ForceUnit" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                 <div class="form-group"  >
                                    <label ID="Label1" class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">Temperature Unit</label>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <asp:DropDownList ID="ddl_TempUnit" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                 <div class="form-group"  >
                                    <label ID="lbl_CoordinatesSystem" class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">Coordinate System</label>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <asp:DropDownList ID="ddl_CoordinatesSystem" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
<%--                                         <asp:Button ID="btn_SkipAll" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right"/>--%>
                                        <asp:Button ID="btn_SkipAll3" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right"/>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div id="step-2" class="wizard_content" style="display: none;">
                            <div class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        First Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_FName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Last Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_LName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Middle Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_MName" runat="server" CssClass="form-control col-md-7 col-xs-12"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_Address" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_Role" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_OrganizationName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_OrganizationAddress" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Owner_OrganizationRole" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                                         <asp:Button ID="btn_SkipAll2" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right"/>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div id="step-3" class="wizard_content" style="display: none;">
                            <div class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        First Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_FName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Last Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_LName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Middle Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_MName" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_Address" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_Role" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_OrganizationName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_OrganizationAddress" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Designer_OrganizationRole" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                                         <asp:Button ID="btn_SkipAll1" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right"/>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div id="step-4" class="wizard_content" style="display: none;">
                            <div class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        First Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_FName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Last Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_LName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Middle Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_MName" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_Address" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_Role" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_OrganizationName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_OrganizationAddress" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Contractor_OrganizationRole" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                                         <asp:Button ID="btn_SkipAll4" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="step-5" class="wizard_content" style="display: none;">
                            <div class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        First Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_FName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Last Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_LName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Middle Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_MName" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_Address" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_Role" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Name<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_OrganizationName" runat="server" CssClass="form-control col-md-7 col-xs-12" required="required" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Address<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_OrganizationAddress" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-3 col-sm-3 col-xs-12" for="Project-Title">
                                        Organization Role<span class="required">  </span>
                                    </label>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <asp:TextBox ID="txt_Consultant_OrganizationRole" runat="server" CssClass="form-control col-md-7 col-xs-12" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                                         <asp:Button ID="btn_SkipAll_Last" runat="server" Text="Finish" CssClass="btn btn-danger"  OnClick="btn_Finish_Click" style="float:right" CausesValidation="false"/>
                                    </div>
                                </div>
                            </div>
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
