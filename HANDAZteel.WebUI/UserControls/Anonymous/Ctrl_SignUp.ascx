<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_SignUp.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Anonymous.Ctrl_SignUp" %>

  <script src="js/validator/validator.js"></script>
<div class="col-md-6 col-xs-12">
    <div class="x_panel">
        <div class="x_title">
            <h2>Create New Account<small> @HANDAZ.com</small></h2>
            <ul class="nav navbar-right panel_toolbox">
                <li>
                    <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                    <ul class="dropdown-menu" role="menu">
                        <li>
                            <a href="#">Register</a>
                        </li>
                        <li>
                            <a href="../../Pages/Anonymous/Login.aspx">Log In(already have account)</a>
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



<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" CompleteSuccessText="Your HANDAZ account has been successfully created." CreateUserButtonText="Register"  OnCreatedUser="CreateUserWizard2_CreatedUser" UnknownErrorMessage="Your HANDAZ  account was not created. Please try again." OnCancelButtonClick="btn_Cancel_Click" OnContinueButtonClick="ContinueButton_Click">
    <WizardSteps>
        <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            <ContentTemplate>



                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User-name</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="UserName" runat="server" required="required" type="text"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="Password" runat="server" TextMode="Password" required="required" type="password"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm-Password</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="ConfirmPassword" runat="server" TextMode="Password" required="required" type="password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="EmailLabel" runat="server" AssociatedControlID="Email" >E-mail:</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="Email" runat="server" required="required" type="email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="QuestionLabel" runat="server" AssociatedControlID="Question"  >Security-Question</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="Question" runat="server" required="required" type="text"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="Question" ErrorMessage="Security question is required." ToolTip="Security question is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label CssClass="control-label col-md-3 col-sm-3 col-xs-12" ID="AnswerLabel" runat="server" AssociatedControlID="Answer"  >Security-Answer</asp:Label>
                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                        <asp:TextBox CssClass="form-control" ID="Answer" runat="server" required="required" type="text"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer" ErrorMessage="Security answer is required." ToolTip="Security answer is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match." ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                </div>




                            </ContentTemplate>
        </asp:CreateUserWizardStep>
        <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
        </asp:CompleteWizardStep>
    </WizardSteps>
</asp:CreateUserWizard>


            </div>
        </div>
    </div>

</div>
  <script>
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
  </script>













