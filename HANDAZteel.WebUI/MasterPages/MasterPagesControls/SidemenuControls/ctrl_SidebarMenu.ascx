<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_SidebarMenu.ascx.cs" Inherits="HANDAZ.PEB.WebUI.MasterPages.MasterPagesControls.SidemenuControls.SidebarMenu" %>
 <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">

                        <div class="menu_section">
                            <h3>Menu</h3>
                            <ul class="nav side-menu">
                                <li><a><i class="fa fa-home"></i> Profile <span class="fa fa-chevron-down"></span></a>
                                    <ul class="nav child_menu" style="display: none">
                                        <li><a href="../../../Pages/Designer/Profile.aspx">View Profile</a>
                                        </li>
                                        <li><a href="../../../Pages/Designer/EditProfile.aspx">Edit Profile</a>
                                        </li>
                                    </ul>
                                </li>
                                <li><a><i class="fa fa-edit"></i> Projects <span class="fa fa-chevron-down"></span></a>
                                    <ul class="nav child_menu" style="display: none">
                                        <li><a href="../../../Pages/Designer/NewProjectInformation.aspx">New Project</a>
                                        </li>
                                        <li><a href="../../../Pages/Designer/ManageProjects.aspx">Manage Projects</a>
                                        </li>
                                    </ul>
                                </li>
                                <%--  <li><a><i class="fa fa-files-o"></i> Documents <span class="fa fa-chevron-down"></span></a>
                                    <ul class="nav child_menu" style="display: none">
                                        <li><a href="../../../Pages/Designer/NewProjectInformation.aspx">New Project</a>
                                        </li>
                                        <li><a href="../../../Pages/Designer/ManageProjects.aspx">Manage Projects</a>
                                        </li>
                                    </ul>
                                </li>--%>
                            </ul>
                        </div>
                    </div>