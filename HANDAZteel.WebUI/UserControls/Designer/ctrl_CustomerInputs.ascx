<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_CustomerInputs.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_CustomerInputs" %>
<%--<script src="../../validator.js"></script>--%>

<script>
   <%-- $(document).on("focusin", '<%= txt_BaySpacing.ClientID %>', function (event) {

        $(this).prop('readonly', true);

    });

    $(document).on("focusout", '<%= txt_BaySpacing.ClientID %>', function (event) {

        $(this).prop('readonly', false);

    });--%>
    function LengthChanged() {
        var length = document.getElementById('<%= txt_LandLength.ClientID %>').value;

        var nFrames = document.getElementById('<%= txt_FramesCount.ClientID %>').value;
        var baySpacing = document.getElementById('<%= txt_BaySpacing.ClientID %>').value = length / (nFrames - 1)
        if (baySpacing < 1) {
            document.getElementById('<%= txt_BaySpacing.ClientID %>').value = "";

        }
    }
</script>
<div class="col-md-6 col-xs-12">
    <div class="x_panel">
        <div class="x_title">
            <h2>Building Information</h2>
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
        <div class="x_content" style="display: block;">
            <br>
            <div class="form-horizontal form-label-left">

                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_Location" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Location</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_Location" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_LandWidth" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Width (b)</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_LandWidth" runat="server" CssClass="form-control" required="required" type="number"></asp:TextBox>
                    </div>
                    <asp:Label ID="lbl_UnitLandWidth" runat="server" Text="meter" CssClass="control-label col-md-1 col-sm-3 col-xs-12"></asp:Label>
                </div>

                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_LandLength" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Length (d)</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_LandLength" name="txt_LandLength" onKeyPress="javascript:LengthChanged();" onchange="javascript:LengthChanged();" runat="server" CssClass="form-control" required="required" type="number"></asp:TextBox>
                    </div>

                    <asp:Label ID="lbl_UnitLandLength" OnTextChanged="ChangeBaySpacing" runat="server" Text="meter" CssClass="control-label col-md-1 col-sm-3 col-xs-12"></asp:Label>

                </div>


                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_EaveHeight" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Eave Height (hcr)</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_EaveHeight" runat="server" CssClass="form-control" required="required" type="number"></asp:TextBox>
                    </div>

                    <asp:Label ID="lbl_UnitEaveHeight" runat="server" Text="meter" CssClass="control-label col-md-1 col-sm-3 col-xs-12"></asp:Label>
                </div>


                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_RoofSlope" runat="server" Text="Label" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Roof Slope </asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_RoofSlope" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group" style="margin-bottom: 10px">
                    <div class="col-md-9 col-sm-9 col-xs-12">

                        <asp:RadioButton ID="rd_BaySpacing" runat="server" Text="Bay Spacing" GroupName="BaySpacing" Enabled="False" />
                        <asp:RadioButton ID="rd_NoFrames" runat="server" Text="Number of Frames" GroupName="BaySpacing" Enabled="False" Checked="True" />
                    </div>
                </div>

                <div class="form-group" style="margin-bottom: 30px">

                    <asp:Label ID="lbl_BaySpacing" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Bay Spacing (db)</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_BaySpacing" runat="server" CssClass="form-control" required="required" type="number" data-validate-minmax="5,10" CausesValidation="True" ReadOnly="False"></asp:TextBox>
                    </div>
                    <asp:Label ID="lbl_UnitBaySpacing" runat="server" Text="meter" CssClass="control-label col-md-1 col-sm-3 col-xs-12"></asp:Label>
                </div>


                <div class="form-group" style="margin-bottom: 30px">

                    <asp:Label ID="lbl_FramesCount" runat="server" CssClass="control-label col-md-4 col-sm-3 col-xs-12" Enabled="False">Number of Frames (n)</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_FramesCount" runat="server" onKeyPress="javascript:LengthChanged();" onchange="javascript:LengthChanged();" CssClass="form-control" required="required" type="number" TextMode="Number"></asp:TextBox>
                    </div>

                </div>

                <div class="form-group" style="margin-bottom: 30px">

                    <asp:Label ID="lbl_RoofAccessability" runat="server" Text="Label" CssClass="control-label col-md-4 col-sm-3 col-xs-12">Roof Accessibility</asp:Label>
                    <div class="col-md-4 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_RoofAccessability" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 30px">
                    <asp:Label ID="lbl_ErrorBaySpacing" runat="server" CssClass="control-label col-md-12 col-sm-3 col-xs-12" ForeColor="#CC0000" Visible="False" Text="Change number of frames  or  frame length to have a valid bay spacing "></asp:Label>
                </div>

                <div class="ln_solid"></div>
                <div class="form-group">
                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btn_Cancel_Click" />
                        <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="btn_Submit_Click" />
                        <asp:Button ID="btn_Preview" runat="server" Text="Preview" CssClass="btn btn-danger" OnClick="btn_Preview_Click" />
                        <button type="button" id="btn_error" name="btn_error" class="btn btn-danger" data-toggle="modal" data-target=".bs-example-modal-sm" style="visibility: <%=InvalidModel()%>">Preview</button>

                    </div>
                </div>

            </div>



            <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                            <h4 class="modal-title" id="myModalLabel2">Invalid Inputs</h4>
                        </div>
                        <div class="modal-body">
                            <%-- <h4>Illogical Inputs</h4>--%>
                            <p>your frame inputs are invalid.</p>
                            <p>Please correct your input data to generate a valid model .</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                        </div>
                        <!-- Small modal -->
                    </div>
                </div>
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
      .on('keyup blur', 'input', function() {
        validator.checkField.apply($(this).siblings().last()[0]);
      });

    // bind the validation to the form submit event
    //$('#send').click('submit');//.prop('disabled', true);

    $('form').submit(function(e) {
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
    $('#vfields').change(function() {
      $('form').toggleClass('mode2');
    }).prop('checked', false);

    $('#alerts').change(function() {
      validator.defaults.alerts = (this.checked) ? false : true;
      if (this.checked)
        $('form .alert').remove();
    }).prop('checked', false);
  </script>--%>
<%--<script>
    function btn_Cancel_Click()
    {
        document.getElementById("txt_BaySpacing").textContent = "";
        document.getElementById("txt_EaveHeight").textContent ="";
        document.getElementById("txt_LandLength").textContent ="";
        document.getElementById("txt_LandWidth").textContent= "";
    }
</script>--%>