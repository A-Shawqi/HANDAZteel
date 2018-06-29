<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_Viewer.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_Viewer" %>
<script>
    $(document).ready(function () {
        //declare viewer and browser at the beginning so that it can be used as a variable before it is initialized.
        var viewer = null;
        var browser = null;

        function initControls() {

            $("#semantic-descriptive-info").accordion({
                heightStyle: "fill"
            });
            $("#semantic-model").accordion({
                heightStyle: "fill"
            });

            $("#btnLocate").button().click(function () {
                var id = $(this).data("id");
                if (typeof (id) != "undefined" && viewer) {
                    viewer.zoomTo(parseInt(id));
                }
            });

            $("#toolbar button").button();

        }
        function reinitControls() {
            $("#semantic-model").accordion("refresh");
            $("#semantic-descriptive-info").accordion("refresh");
        }
        initControls();
        $(window).resize(function () {
            reinitControls();
        });

        var keepTarget = false;
        browser = new xBrowser();
        browser.on("loaded", function (args) {
            var facility = args.model.facility;
            //render parts
            browser.renderSpatialStructure("structure", true);
            browser.renderAssetTypes("assetTypes", true);
            browser.renderSystems("systems");
            browser.renderZones("zones");
            browser.renderContacts("contacts");
            browser.renderDocuments(facility[0], "facility-documents");

            //open and selectfacility node
            $("#structure > ul > li").click();
        });

        browser.on("entityClick", function (args) {
            var span = $(args.element).children("span.xbim-entity");
            if (document._lastSelection)
                document._lastSelection.removeClass("ui-selected");
            span.addClass("ui-selected")
            document._lastSelection = span;
        });
        browser.on("entityActive", function (args) {
            var isRightPanelClick = false;
            if (args.element)
                if ($(args.element).parents("#semantic-descriptive-info").length != 0)
                    isRightPanelClick = true;

            //set ID for location button
            $("#btnLocate").data("id", args.entity.id);

            browser.renderPropertiesAttributes(args.entity, "attrprop");
            browser.renderAssignments(args.entity, "assignments");
            browser.renderDocuments(args.entity, "documents");
            browser.renderIssues(args.entity, "issues");

            if (isRightPanelClick)
                $("#attrprop-header").click();

        });

        browser.on("entityDblclick", function (args) {
            var entity = args.entity;
            var allowedTypes = ["space", "assettype", "asset"];
            if (allowedTypes.indexOf(entity.type) === -1) return;

            var id = parseInt(entity.id);
            if (id && viewer) {
                viewer.resetStates();
                viewer.renderingMode = "x-ray";
                if (entity.type === "assettype") {
                    var ids = [];
                    for (var i = 0; i < entity.children.length; i++) {
                        id = parseInt(entity.children[i].id);
                        ids.push(id);
                    }
                    viewer.setState(xState.HIGHLIGHTED, ids);
                }
                else {
                    viewer.setState(xState.HIGHLIGHTED, [id]);
                }
                viewer.zoomTo(id);
                keepTarget = true;
            }
        });


        //viewer set up
        var check = xViewer.check();
        if (check.noErrors) {
            //alert('WebGL support is OK');
            viewer = new xViewer("viewer-canvas");
            viewer.background = [249, 249, 249, 255];
            viewer.on("mouseDown", function (args) {
                if (!keepTarget) viewer.setCameraTarget(args.id);
            });
            viewer.on("pick", function (args) {
                browser.activateEntity(args.id);
                viewer.renderingMode = "normal";
                viewer.resetStates();
                keepTarget = false;
            });
            viewer.on("dblclick", function (args) {
                viewer.resetStates();
                viewer.renderingMode = "x-ray";
                var id = args.id;
                viewer.setState(xState.HIGHLIGHTED, [id]);
                viewer.zoomTo(id);
                keepTarget = true;
            });

            viewer.load("<%=getWexbimFullPath()%>");
                //browser.load("../../xBIM-WebUI/tests/data/ITI_B148.json");


                //var cube = new xNavigationCube();
                //viewer.addPlugin(cube);

                viewer.start();
            }
            else {
                alert("WebGL support is unsufficient");
                var msg = document.getElementById("msg");
                msg.innerHTML = "";
                for (var i in check.errors) {
                    if (check.errors.hasOwnProperty(i)) {
                        var error = check.errors[i];
                        msg.innerHTML += "<div style='color: red;'>" + error + "</div>";
                    }
                }
            }
        });
</script>


<script>
    $(document).ready(function () {
        $("#hide").click(function () {
            $("#semantic-model-container").toggle();
            $("#semantic-descriptive-info-container").toggle();

        });

    });
</script>






<script>(function main() {
    // Create enabled event
    function fireEnabledEvent() {
        // If gli exists, then we are already present and shouldn't do anything
        if (!window.gli) {
            setTimeout(function () {
                var enabledEvent = document.createEvent("Event");
                enabledEvent.initEvent("WebGLEnabledEvent", true, true);
                document.dispatchEvent(enabledEvent);
            }, 0);
        } else {
            //console.log("WebGL Inspector already embedded on the page - disabling extension");
        }
    };

    // Grab the path root from the extension
    document.addEventListener("WebGLInspectorReadyEvent", function (e) {
        var pathElement = document.getElementById("__webglpathroot");
        if (window["gliloader"]) {
            gliloader.pathRoot = pathElement.innerText;
        } else {
            // TODO: more?
            window.gliCssUrl = pathElement.innerText + "gli.all.css";
        }
    }, false);

    // Rewrite getContext to snoop for webgl
    var originalGetContext = HTMLCanvasElement.prototype.getContext;
    if (!HTMLCanvasElement.prototype.getContextRaw) {
        HTMLCanvasElement.prototype.getContextRaw = originalGetContext;
    }
    HTMLCanvasElement.prototype.getContext = function () {
        var ignoreCanvas = this.internalInspectorSurface;
        if (ignoreCanvas) {
            return originalGetContext.apply(this, arguments);
        }

        var result = originalGetContext.apply(this, arguments);
        if (result == null) {
            return null;
        }

        var contextNames = ["moz-webgl", "webkit-3d", "experimental-webgl", "webgl", "3d"];
        var requestingWebGL = contextNames.indexOf(arguments[0]) != -1;
        if (requestingWebGL) {
            // Page is requesting a WebGL context!
            fireEnabledEvent(this);

            // If we are injected, inspect this context
            if (window.gli) {
                if (gli.host.inspectContext) {
                    // TODO: pull options from extension
                    result = gli.host.inspectContext(this, result);
                    // NOTE: execute in a timeout so that if the dom is not yet
                    // loaded this won't error out.
                    window.setTimeout(function () {
                        var hostUI = new gli.host.HostUI(result);
                        result.hostUI = hostUI; // just so we can access it later for debugging
                    }, 0);
                }
            }
        }

        return result;
    };
})();</script>

<div class="col-md-100 col-xs-100 col-lg-100">
    <div class="x_panel">
        <div class="x_title">
            <h2>BIM Model</h2>
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
            <form class="form-horizontal form-label-left">

                <div id="viewer-container">
                    <canvas id="viewer-canvas" style="width:100%; height:100%"></canvas>
                    <div style="position: absolute; left: 5px; top: 5px; padding: 5px;" id="toolbar" class="ui-widget-header ui-corner-all">
                        <button onclick=" viewer.clip(); " class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button"><span class="ui-button-text">Clip</span></button>
                        <button onclick=" viewer.unclip(); " class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button"><span class="ui-button-text">Unclip</span></button>
                    </div>
                </div>

            </form>
        </div>
    </div>
</div>




