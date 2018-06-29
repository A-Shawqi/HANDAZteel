<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IFCViewerFull.aspx.cs" Inherits="HANDAZ.PEB.WebUI.Pages.Designer.IFCViewerFull" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <!DOCTYPE html>
<!-- saved from url=(0014)about:internet -->
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WeXplorer - xBIM Web Viewer</title>
       <link href="/xBIM-WebUI/Libs/jquery-ui-styles/jquery-ui.min.css" rel="stylesheet">
    <link href="/xBIM-WebUI/Resources/doctemplate/static/styles/xbrowser-styles.css" rel="stylesheet">

    <script src="/xBIM-WebUI/Libs/jquery.js"></script>
    <script src="/xBIM-WebUI/Libs/jquery-ui.js"></script>
    <script src="/xBIM-WebUI/Libs/webgl-utils.js"></script>
    <script src="/xBIM-WebUI/Libs/gl-matrix.js"></script>
    <!--<script src="/xBIM-WebUI/Build/xbim-browser.js"></script>
    <script src="/xBIM-WebUI/Build/xbim-viewer.debug.bundle.js"></script>-->
    <script src="/xBIM-WebUI/Viewer/xbim-binary-reader.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-model-geometry.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-model-handle.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-product-type.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-shaders.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-state.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-triangulated-shape.debug.js"></script>
    <script src="/xBIM-WebUI/Viewer/xbim-viewer.debug.js"></script>

    <script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube-shaders.debug.js"></script>
    <script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube-textures.debug.js"></script>
    <script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube.debug.js"></script>

    <script src="/xBIM-WebUI/Browser/xbim-attribute-dictionary.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-browser.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-cobie-utils.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-cobieuk-utils.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-attribute.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-entity.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-model.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-property.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-templates.debug.js"></script>
    <script src="/xBIM-WebUI/Browser/xbim-visual-assignment-set.debug.js"></script>
    <script src="/xBIM-WebUI/browser.js"></script>

    
</head>
<body>
    <script>
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
            browser.load("<%=GetJsonFullPath()%>");


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


<div id="viewer-container">
    <canvas id="viewer-canvas"></canvas>
    <div style="position: absolute; left: 5px; top: 5px; padding: 5px;" id="toolbar" class="ui-widget-header ui-corner-all">
        <button onclick=" viewer.clip(); ">Clip</button>
        <button onclick=" viewer.unclip(); ">Unclip</button>
    </div>
</div>
<div id="semantic-model-container" class="noselect">
    <div id="semantic-model">
        <h3>Spatial structure</h3>
        <div class="no-overflow">
            <div id="structure" class="semantic-model-tree"></div>
        </div>
        <h3>Asset types</h3>
        <div class="no-overflow">
            <div id="assetTypes" class="semantic-model-tree"></div>
        </div>
        <h3>Systems</h3>
        <div class="no-overflow">
            <div id="systems" class="semantic-model-tree"></div>
        </div>
        <h3>Zones</h3>
        <div class="no-overflow">
            <div id="zones" class="semantic-model-tree"></div>
        </div>
        <h3>Contacts</h3>
        <div class="no-overflow">
            <div id="contacts" class="semantic-model-tree"></div>
        </div>
        <h3>Documents</h3>
        <div class="no-overflow">
            <div id="facility-documents" class="semantic-model-tree"></div>
        </div>
    </div>
</div>

<div id="semantic-descriptive-info-container">
    <div id="semantic-descriptive-info">
        <h3 id="attrprop-header">Properties and attributes </h3>
        <div class="no-overflow-y">
            <div id="attrprop"></div>
        </div>
        <h3>Documents</h3>
        <div class="no-overflow">
            <div id="documents"></div>
        </div>
        <h3>Tasks</h3>
        <div class="no-overflow">
            <div id="issues"></div>
        </div>
        <h3>Assignments</h3>
        <div class="no-overflow">
            <div id="assignments"></div>
        </div>
    </div>
</div>
<span class="right" id="btnLocate"><span class="ui-icon ui-icon-pin-s left"></span> Locate</span>
</body>
</html>
    </div>
    </form>
</body>
</html>
