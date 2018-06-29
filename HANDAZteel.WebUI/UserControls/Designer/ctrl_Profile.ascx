<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_Profile.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_Profile" %>







<!-- top navigation -->

<!-- /top navigation -->

<!-- page content -->

<div class="page-title">
    <div class="title_left">
        <h3>Profile</h3>
    </div>

    <div class="title_right">
        <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
            <div class="input-group">
                <input type="text" class="form-control" placeholder="Search for...">
                <span class="input-group-btn">
                    <button class="btn btn-default" type="button">Go!</button>
                </span>
            </div>
        </div>
    </div>
</div>
<div class="clearfix"></div>

<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>Activity Report <small>generated models report</small></h2>
                <ul class="nav navbar-right panel_toolbox">
                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#">View Profile</a>
                            </li>
                            <li><a href="../../Pages/Designer/EditProfile.aspx">Edit Profile</a>
                            </li>
                        </ul>
                    </li>
                    <li><a class="close-link"><i <%--class="fa fa-close"--%>></i></a>
                    </li>
                </ul>
                <div class="clearfix"></div>
            </div>
            <div class="x_content">

                <div class="col-md-3 col-sm-3 col-xs-12 profile_left">

                    <div class="profile_img">

                        <!-- end of image cropping -->
                        <div id="crop-avatar">
                            <!-- Current avatar -->
                            <img class="img-responsive avatar-view" src="<%=GetImageURL() %>" alt="Avatar" title="Change your Picture">

                            <!-- Cropping modal -->
                            <div class="modal fade" id="avatar-modal" aria-hidden="true" aria-labelledby="avatar-modal-label" role="dialog" tabindex="-1">
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
                            </div>
                            <!-- /.modal -->

                            <!-- Loading state -->
                            <div class="loading" aria-label="Loading" role="img" tabindex="-1"></div>
                        </div>
                        <!-- end of image cropping -->

                    </div>
                    <h3><%=GetUserFullName() %></h3>

                    <ul class="list-unstyled user_data">
                        <li>

                            <i class="fa fa-map-marker user-profile-icon"></i><%=GetUserAddress() %>
                      </li>
                        <li>
                             <%-- <div class="fa-hover col-md-3 col-sm-4 col-xs-12"><a ><i class="fa fa-mobile-phone"></i></a>
                      </div>--%>
                            <i class="fa fa-phone"></i><%=GetUserPhone() %>
                      </li>

                        <li>
                            <i class="fa fa-briefcase user-profile-icon"></i><%=GetUserJop() %>
                      </li>
                        <li>
                            <i class="fa fa-briefcase user-profile-icon"></i><%=GetUserCompany() %>
                      </li>


                        <li class="m-top-xs">
                            <i class="fa fa-external-link user-profile-icon"></i>
                            <a><%=GetUserEmail() %></a>
                        </li>
                    </ul>

                    <a class="btn btn-success" href="../../Pages/Designer/EditProfile.aspx"><i class="fa fa-edit m-right-xs"></i>Edit Profile</a>
                    <br />

                    <!-- start skills -->
                    <h4>Development</h4>
                    <ul class="list-unstyled user_data">
                        <li>
                            <p>Full Designed Models</p>
                            <div class="progress progress_sm">
                                <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="50"></div>
                            </div>
                        </li>
                        <li>
                            <p>Partial Designed Models</p>
                            <div class="progress progress_sm">
                                <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="70"></div>
                            </div>
                        </li>
                        <li>
                            <p>IFC Downloaded Models</p>
                            <div class="progress progress_sm">
                                <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="30"></div>
                            </div>
                        </li>
                        <li>
                            <p>DWG Downloaded Drawings</p>
                            <div class="progress progress_sm">
                                <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="50"></div>
                            </div>
                        </li>
                    </ul>
                    <!-- end of skills -->

                </div>
                <div class="col-md-9 col-sm-9 col-xs-12">

                    <div class="profile_title">
                        <div class="col-md-6">
                            <h2>Generated Models Report</h2>
                        </div>
                        <div class="col-md-6">
                            <div id="reportrange" class="pull-right" style="margin-top: 5px; background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #E6E9ED">
                                <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>
                                <span>December 30, 2015 - January 28, 2017</span> <b class="caret"></b>
                            </div>
                        </div>
                    </div>
                    <!-- start of user-activity-graph -->
                    <div id="graph_bar" style="width: 100%; height: 280px;"></div>
                    <!-- end of user-activity-graph -->

                    <div class="" role="tabpanel" data-example-id="togglable-tabs">
                         <ul id="myTab" class="nav nav-tabs bar_tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">Projects Worked on</a>
                        </li>
                        <li role="presentation" class=""><a href="#tab_content2" role="tab" id="profile-tab" data-toggle="tab" aria-expanded="false">Recent Activity</a>
                        </li>
                        <li role="presentation" class=""><a href="#tab_content3" role="tab" id="profile-tab2" data-toggle="tab" aria-expanded="false">Profile</a>
                        </li>
                      </ul>
                        <div id="myTabContent" class="tab-content">
                        <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">

                                <!-- start user projects -->
                                <table class="data table table-striped no-margin">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Model Name</th>
                                            <th>Client Company</th>
                                            <th class="hidden-phone">Minutes Spent</th>
                                            <th>Download IFC</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1</td>
                                            <td>BMW Factory</td>
                                            <td>El-Zamil Inc</td>
                                            <td class="hidden-phone">3</td>
                                            <td class="vertical-align-mid">
                                                <div class="progress">
                                                    <div class="progress-bar progress-bar-success" data-transitiongoal="35"></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>2</td>
                                            <td>Guide Building</td>
                                            <td>Dar El-Handsa</td>
                                            <td class="hidden-phone">13</td>
                                            <td class="vertical-align-mid">
                                                <div class="progress">
                                                    <div class="progress-bar progress-bar-danger" data-transitiongoal="15"></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>3</td>
                                            <td>CNC Hall</td>
                                            <td>Arab Contractor</td>
                                            <td class="hidden-phone">8</td>
                                            <td class="vertical-align-mid">
                                                <div class="progress">
                                                    <div class="progress-bar progress-bar-success" data-transitiongoal="45"></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>4</td>
                                            <td>Sweets Factory</td>
                                            <td>C.C.C</td>
                                            <td class="hidden-phone">8</td>
                                            <td class="vertical-align-mid">
                                                <div class="progress">
                                                    <div class="progress-bar progress-bar-success" data-transitiongoal="75"></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <!-- end user projects -->


                            </div>
                        <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">

                                <!-- start recent activity -->
                                <ul class="messages">
                                    <li>
                                        <img src="<%=GetImageURL() %>" class="avatar" alt="Avatar">
                                        <div class="message_date">
                                            <h3 class="date text-info">24</h3>
                                            <p class="month">May</p>
                                        </div>
                                        <div class="message_wrapper">
                                            <h4 class="heading">Desmond Davison</h4>
                                            <blockquote class="message">Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua butcher retro keffiyeh dreamcatcher synth.</blockquote>
                                            <br />
                                            <p class="url">
                                                <span class="fs1 text-info" aria-hidden="true" data-icon=""></span>
                                                <a href="#"><i class="fa fa-paperclip"></i>User Acceptance Test.ifc </a>
                                            </p>
                                        </div>
                                    </li>
                                    <li>
                                        <img src="<%=GetImageURL() %>" class="avatar" alt="Avatar">
                                        <div class="message_date">
                                            <h3 class="date text-error">21</h3>
                                            <p class="month">May</p>
                                        </div>
                                        <div class="message_wrapper">
                                            <h4 class="heading">Brian Michaels</h4>
                                            <blockquote class="message">Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua butcher retro keffiyeh dreamcatcher synth.</blockquote>
                                            <br />
                                            <p class="url">
                                                <span class="fs1" aria-hidden="true" data-icon=""></span>
                                                <a href="#" data-original-title="">Download</a>
                                            </p>
                                        </div>
                                    </li>
                                    <li>
                                        <img src="<%=GetImageURL() %>" class="avatar" alt="Avatar">
                                        <div class="message_date">
                                            <h3 class="date text-info">24</h3>
                                            <p class="month">May</p>
                                        </div>
                                        <div class="message_wrapper">
                                            <h4 class="heading">Desmond Davison</h4>
                                            <blockquote class="message">Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua butcher retro keffiyeh dreamcatcher synth.</blockquote>
                                            <br />
                                            <p class="url">
                                                <span class="fs1 text-info" aria-hidden="true" data-icon=""></span>
                                                <a href="#"><i class="fa fa-paperclip"></i>User Acceptance Test.doc </a>
                                            </p>
                                        </div>
                                    </li>
                                    <li>
                                        <img src="<%=GetImageURL() %>" class="avatar" alt="Avatar">
                                        <div class="message_date">
                                            <h3 class="date text-error">21</h3>
                                            <p class="month">May</p>
                                        </div>
                                        <div class="message_wrapper">
                                            <h4 class="heading">Brian Michaels</h4>
                                            <blockquote class="message">Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua butcher retro keffiyeh dreamcatcher synth.</blockquote>
                                            <br />
                                            <p class="url">
                                                <span class="fs1" aria-hidden="true" data-icon=""></span>
                                                <a href="#" data-original-title="">Download</a>
                                            </p>
                                        </div>
                                    </li>

                                </ul>
                                <!-- end recent activity -->

                            </div>
                            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab">
                                <p>
                                    xxFood truck fixie locavore, accusamus mcsweeney's marfa nulla single-origin coffee squid. Exercitation +1 labore velit, blog sartorial PBR leggings next level wes anderson artisan four loko farm-to-table craft beer twee. Qui
                            photo booth letterpress, commodo enim craft beer mlkshk
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- /page content -->

<!-- footer content -->

<!-- /footer content -->

<div id="custom_notifications" class="custom-notifications dsp_none">
    <ul class="list-unstyled notifications clearfix" data-tabbed_notifications="notif-group">
    </ul>
    <div class="clearfix"></div>
    <div id="notif-group" class="tabbed_notifications"></div>
</div>


<!-- /datepicker -->
