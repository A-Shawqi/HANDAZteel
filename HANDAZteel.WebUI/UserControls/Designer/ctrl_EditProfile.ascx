<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_EditProfile.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_EditProfile" %>


<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>Edit your profile picture<small>upload new picture</small></h2>
                <ul class="nav navbar-right panel_toolbox">
                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="../../Pages/Designer/Profile.aspx">View Profile</a>
                            </li>
                            <li><a href="#">Edit Profile</a>
                            </li>
                        </ul>
                    </li>
                    <li><a class="close-link"><i <%--class="fa fa-close"--%>></i></a>
                    </li>
                </ul>
                <div class="clearfix"></div>
            </div>
            <div class="x_content">
                <div class="form-group">
                    <div class="col-md-3 col-sm-3 col-xs-12 profile_left">

                        <div class="profile_img">

                            <!-- end of image cropping -->
                            <div id="crop-avatar">
                                <!-- Current avatar -->
                                <asp:Image ID="img_ProfilePic" runat="server" CssClass="img-responsive avatar-view" ImageUrl="~/images/Profile Pictures/TempPicture.jpg" />

                                <!-- Cropping modal -->
                                <%--                            <div class="modal fade" id="avatar-modal" aria-hidden="true" aria-labelledby="avatar-modal-label" role="dialog" tabindex="-1">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="avatar-form" action="crop.php" enctype="multipart/form-data" method="post">
                                            <div class="modal-header">
                                                <button class="close" data-dismiss="modal" type="button">&times;</button>
                                                <h4 class="modal-title" id="avatar-modal-label">Change Picture</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="avatar-body">

                                                    <!-- Upload image and data -->
                                                    <div class="avatar-upload">
                                                        <input class="avatar-src" name="avatar_src" type="hidden">
                                                        <input class="avatar-data" name="avatar_data" type="hidden">
                                                        <label for="avatarInput">Local upload</label>
                                                        <input class="avatar-input" id="avatarInput" name="avatar_file" type="file">
                                                    </div>

                                                    <!-- Crop and preview -->
                                                    <div class="row">
                                                        <div class="col-md-9">
                                                            <div class="avatar-wrapper"></div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="avatar-preview preview-lg"></div>
                                                            <div class="avatar-preview preview-md"></div>
                                                            <div class="avatar-preview preview-sm"></div>
                                                        </div>
                                                    </div>

                                                    <div class="row avatar-btns">
                                                        <div class="col-md-9">
                                                            <div class="btn-group">
                                                                <button class="btn btn-primary" data-method="rotate" data-option="-90" type="button" title="Rotate -90 degrees">Rotate Left</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="-15" type="button">-15deg</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="-30" type="button">-30deg</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="-45" type="button">-45deg</button>
                                                            </div>
                                                            <div class="btn-group">
                                                                <button class="btn btn-primary" data-method="rotate" data-option="90" type="button" title="Rotate 90 degrees">Rotate Right</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="15" type="button">15deg</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="30" type="button">30deg</button>
                                                                <button class="btn btn-primary" data-method="rotate" data-option="45" type="button">45deg</button>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <button class="btn btn-primary btn-block avatar-save" type="submit">Done</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- <div class="modal-footer">
                                                  <button class="btn btn-default" data-dismiss="modal" type="button">Close</button>
                                                </div> -->
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                                <!-- /.modal -->

                                <!-- Loading state -->
                                <div class="loading" aria-label="Loading" role="img" tabindex="-1"></div>
                            </div>
                            <!-- end of image cropping -->

                        </div>
                        <h3><%=GetUserFullName() %></h3>

                        <ul class="list-unstyled user_data">
                        </ul>
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="control-label" />
                        <asp:Button ID="btn_upload" runat="server" Text="Upload" OnClick="btn_upload_Click" CssClass="btn btn-primary btn-block avatar-save" />

                    </div>
                </div>
                <br />
                <div class="ln_solid"></div>
                <div class="form-group">
                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                        <asp:Button CssClass="btn btn-primary" ID="btn_Back2Profile" runat="server" Text="Back" OnClick="btn_Back_Click" />
                        <asp:Button CssClass="btn btn-success" ID="btn_SavePic" runat="server" Text="Save Picture" OnClick="btn_SavePic_Click" />
                        <button type="button" class="btn btn-success" data-toggle="modal" data-target=".bs-example-modal-sm" style="visibility: <%=LoggedIn()%>">Save</button>
                    </div>
                </div>
                <br />
            </div>

            <!-- Small modal -->
            <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                            <h4 class="modal-title" id="myModalLabel2">You aren't a member</h4>
                        </div>
                        <div class="modal-body">
                            <h4>We Are Sorry,</h4>
                            <p>you are not logged in and you can't save your info here.</p>
                            <p>Please log in to your account or create a new one if you still haven't account yet .</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                            <a type="button" class="btn btn-success" data-dismiss="modal" href="../../Pages/Anonymous/Login.aspx">Log In</a>
                            <a type="button" class="btn btn-success" data-dismiss="modal" href="../../Pages/Anonymous/SignUp.aspx">Sign Up</a>
                        </div>
                        <!-- Small modal -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="row">
<div class="col-md-6 col-xs-12">
    <div class="x_panel">
        <div class="x_title">
            <h2>Edit your info</h2>
            <ul class="nav navbar-right panel_toolbox">
                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                    <ul class="dropdown-menu" role="menu">
                        <li><a href="#">Edit Profile</a>
                        </li>
                        <li><a href="../../Pages/Designer/Profile.aspx">View Profile</a>
                        </li>
                    </ul>
                </li>
                <li><a class="close-link"><i <%--class="fa fa-close"--%>></i></a>
                </li>
            </ul>
            <div class="clearfix"></div>
        </div>
        <div class="x_content" style="display: block;">
            <br>
            <div class="form-horizontal form-label-left">

                <div class="form-group">
                    <asp:Label ID="lbl_FName" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Full Name</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_FName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>
                <%-- <div class="form-group">
                    <asp:Label ID="lbl_LName" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Last Name</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_LName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>--%>

                <div class="form-group">
                    <asp:Label ID="lbl_Jop" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Jop</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_Jop" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="form-group">
                    <asp:Label ID="lbl_Company" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Company</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_Company" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="form-group">
                    <asp:Label ID="lbl_Phone" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Phone</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_Phone" runat="server" CssClass="form-control" TextMode="Phone"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="form-group">
                    <asp:Label ID="lbl_Address" runat="server" CssClass="control-label col-md-3 col-sm-3 col-xs-12">Address</asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_Address" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="ln_solid"></div>
                <div class="form-group">
                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                        <asp:Button CssClass="btn btn-primary" ID="btn_Back" runat="server" Text="Back" OnClick="btn_Back_Click" />
                        <asp:Button CssClass="btn btn-success" ID="btn_SaveInfo" runat="server" Text="Save Info" OnClick="btn_SaveInfo_Click" />
                        <button type="button" class="btn btn-success" data-toggle="modal" data-target=".bs-example-modal-sm" style="visibility: <%=LoggedIn()%>">Save</button>

                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</div>