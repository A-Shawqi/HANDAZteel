<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_UserSelectiveInputs.ascx.cs" Inherits="HANDAZ.PEB.WebUI.ctrl_UserSelectiveInputs" %>
<div class="col-md-6 col-xs-12">
    <div class="x_panel">
        <div class="x_title">
            <h2>New Building Inputs Form</h2>
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




                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Steel Grade</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_SteelGrade" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <br>

                <div class="form-group" style="margin-bottom: 10px">
                    <div " class="col-md-9 col-sm-9 col-xs-12">
                        <asp:CheckBox ID="chkbox_Purlins" CssClass="iCheck-helper" runat="server" Text="Purlins" />
                    </div>
                </div>
                
                <div class="form-group" style="margin-bottom: 10px">
                    <div  class="col-md-9 col-sm-9 col-xs-12">
                        <asp:RadioButton ID="rd_NoPurlins" runat="server" Text="Number of Purlins" />
                    </div>
                </div>
              
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Number of Left Purlins</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_NoLeftPurlins" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">

                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Number of Right Purlins</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_NoRightPurlins" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                
                <div class="form-group" style="margin-bottom: 10px">

                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:RadioButton ID="rd_PurlinSpacing" runat="server" Text="Purlins Spacing" />
                    </div>
                </div>
               
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Spacing of Left Purlins</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_SpacingLeftPurlins" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Spacing of Right Purlins</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_SpacingRightPurlins" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>


               

                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">d prl</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_prl" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">d prr</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_prr" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                

                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">d pcl</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_pcl" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">d pcr</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_pcr" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="form-group" style="margin-bottom: 10px">

                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Wall bracing Section :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_WallBracingSec" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Wall bracing Material :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_WallBracingMat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                </div>
                <br>


                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Roof bracing Section :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_RoofBracingSec" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Roof bracing Material :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_RoofBracingMat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                </div>
                <br>

                <div class="form-group" style="margin-bottom: 30px">
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:CheckBox ID="chk_CalculateLoads" CssClass="iCheck-helper" runat="server" Text="Calculate Loads Automaticaly" />
                    </div>
                </div>
               

                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Extra D.L :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_ExtraDL" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Extra L.L :</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_ExtraLL" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <br>

                <div class="form-group" style="margin-bottom: 10px">
                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Wind Exposure Category</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_WindCategory" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
               


                <div class="form-group" style="margin-bottom: 10px">

                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Building Category</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:DropDownList ID="ddl_BuildingCategory" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group" style="margin-bottom: 10px">

                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Wind Speed</label>
                    <div class="col-md-9 col-sm-9 col-xs-12">
                        <asp:TextBox ID="txt_WindSpeed" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <br>

                <div class="ln_solid"></div>
                <div class="form-group">
                    <div class="col-md-9 col-sm-9 col-xs-12 col-md-offset-3">
                        <button type="submit" class="btn btn-primary">Cancel</button>
                        <button type="submit" class="btn btn-success">Submit</button>
                        <button type="submit" class="btn btn-danger">Preview</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>
