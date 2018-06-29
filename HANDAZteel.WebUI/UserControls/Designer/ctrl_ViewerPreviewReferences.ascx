<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrl_ViewerPreviewReferences.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.ctrl_ViewerPreviewReferences" %>
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
					window.setTimeout(function() {
						var hostUI = new gli.host.HostUI(result);
						result.hostUI = hostUI; // just so we can access it later for debugging
					}, 0);
				}
			}
		}

		return result;
	};
})();</script>
		<meta http-equiv="content-type" content="text/html; charset=ISO-8859-1">

		<script src="/xBIM-WebUI/Libs/gl-matrix.js"></script>
		<script src="/xBIM-WebUI/Libs/webgl-utils.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-product-type.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-state.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-shaders.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-model-geometry.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-model-handle.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-binary-reader.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-triangulated-shape.debug.js"></script>
		<script type="text/javascript" src="/xBIM-WebUI/Viewer/xbim-viewer.debug.js"></script>
  
		<script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube-shaders.debug.js"></script>
		<script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube.debug.js"></script>
		<script src="/xBIM-WebUI/Plugins/NavigationCube/xbim-navigation-cube-textures.debug.js"></script>

		<script src="/xBIM-WebUI/Plugins/NavigationHome/xbim-navigation-home-textures.debug.js"></script>
		<script src="/xBIM-WebUI/Plugins/NavigationHome/xbim-navigation-home.debug.js"></script>
		<style>
			canvas {
				display: block;
				border: none;
				margin-left: auto;
				margin-right: auto;
			}
		</style>
