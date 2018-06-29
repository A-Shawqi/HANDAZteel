<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_InputsSketchesGuide.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_InputsSketchesGuide" %>


     <div class="col-md-6">
              <div class="x_panel">
                <div class="x_title">
                  <h2>Sketches <small>  </small></h2>
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
                    <li><a <%--class="close-link"--%>><i <%--class="fa fa-close"--%>></i></a>
                    </li>
                  </ul>
                  <div class="clearfix"></div>
                </div>
                <div class="x_content">

                  <div class="row">

                    <%--<p>Frame Sketch</p>--%>

                    <div class="col-md-100">
                      <div <%--class="thumbnail"--%>>
                        <div class="image view view-first">
                          <img style="width: 100%; display: block;" src="/images/Sketches/Frame2D_dimentions.PNG" alt="image" />
                          <div <%--class="mask"--%>>
                            <p>Elevation view</p>
<%--                            <div class="tools tools-bottom">
                              <a ><i class="fa fa-link"></i></a>
                              <a ><i class="fa fa-pencil"></i></a>
                              <a ><i class="fa fa-times"></i></a>
                            </div>--%>
                          </div>
                        </div>
<%--                        <div class="caption">
                          <p>Frame Dimensions </p>
                        </div>--%>
                      </div>


                    <div class="col-md-100">
                      <div <%--class="thumbnail"--%>>
                        <div class="image view view-first">
                          <img style="width: 100%; display: block;" src="/images/Sketches/Frame3D_dimentions.PNG" alt="image" />
                          <div <%--class="mask"--%>>
                            <p>Isometric</p>
<%--                            <div class="tools tools-bottom">
                              <a ><i class="fa fa-link"></i></a>
                              <a ><i class="fa fa-pencil"></i></a>
                              <a ><i class="fa fa-times"></i></a>
                            </div>--%>
                          </div>
                        </div>
                       <%-- <div class="caption">
                          <p>Frame Dimensions </p>
                        </div>--%>
                      </div>
                    </div>
                    </div>
                   

                  </div>

                </div>
              </div>
            </div>