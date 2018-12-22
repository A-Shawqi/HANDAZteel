

/* Copyright (c) 2016, xBIM Team, Northumbria University. All rights reserved.

This javascript library is part of xBIM project. It is provided under the same 
Common Development and Distribution License (CDDL) as the xBIM Toolkit. For 
more information see http://www.openbim.org

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

  * Redistributions of source code must retain the above copyright notice, this
    list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. */
function xBinaryReader() {
    this._buffer = null;
    this._position = 0;
}

xBinaryReader.prototype.onloaded = function () { };
xBinaryReader.prototype.onerror = function () { };

  
xBinaryReader.prototype.load = function (source) {
    this._position = 0;
    var self = this;

    if (typeof (source) == 'undefined' || source == null) throw 'Source must be defined';
    if (typeof (source) == 'string') {
        var xhr;
        xhr = new XMLHttpRequest();
        xhr.open("GET", source, true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var fReader = new FileReader();
                fReader.onloadend = function () {
                    if (fReader.result) {
                        //set data buffer for next processing
                        self._buffer = fReader.result;
                        //do predefined processing of the data
                        if (self.onloaded) {
                            self.onloaded();
                        }
                    }
                };
                fReader.readAsArrayBuffer(xhr.response);
            }
            //throw exception as a warning
            if (xhr.readyState == 4 && xhr.status != 200) {
                var msg = 'Failed to fetch binary data from server. Server code: ' + xhr.status +
                    '. This might be due to CORS policy of your browser if you run this as a local file.';
                if (self.onerror) self.onerror(msg);
                throw msg;
            }
        };
        xhr.responseType = 'blob';
        xhr.send();
    }
    else if (source instanceof Blob || source instanceof File) {
        var fReader = new FileReader();
        fReader.onloadend = function () {
            if (fReader.result) {
                //set data buffer for next processing
                self._buffer = fReader.result;
                //do predefined processing of the data
                if (self.onloaded) {
                    self.onloaded();
                }
            }
        };
        fReader.readAsArrayBuffer(source);
    }
    else if (source instanceof ArrayBuffer) {
        this._buffer = source;
    }
};

xBinaryReader.prototype.getIsEOF = function (type, count) {
    if (typeof (this._position) === "undefined")
        throw "Position is not defined";
    return this._position == this._buffer.byteLength;
};

xBinaryReader.prototype.read = function (arity, count, ctor) {
    if(typeof (count) === "undefined") count = 1;
    var length = arity * count;
    var offset = this._position;
    this._position += length;
    var result;

    return count === 1 ?
        new ctor(this._buffer.slice(offset, offset + length))[0] :
        new ctor(this._buffer.slice(offset, offset + length));
};

xBinaryReader.prototype.readByte = function (count) {
    return this.read(1, count, Uint8Array);
};
xBinaryReader.prototype.readUint8 = function (count) {
    return this.read(1, count, Uint8Array);
};
xBinaryReader.prototype.readInt16 = function (count) {
    return this.read(2, count, Int16Array);
};
xBinaryReader.prototype.readUint16 = function (count) {
    return this.read(2, count, Uint16Array);
};
xBinaryReader.prototype.readInt32 = function (count) {
    return this.read(4, count, Int32Array);
};
xBinaryReader.prototype.readUint32 = function (count) {
    return this.read(4, count, Uint32Array);
};
xBinaryReader.prototype.readFloat32 = function (count) {
    return this.read(4, count, Float32Array);
};
xBinaryReader.prototype.readFloat64 = function (count) {
    return this.read(8, count, Float64Array);
};

//functions for a higher objects like points, colours and matrices
xBinaryReader.prototype.readChar = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var bytes = this.readByte(count);
    var result = new Array(count);
    for (var i in bytes) {
        result[i] = String.fromCharCode(bytes[i]);
    }
    return count ===1 ? result[0] : result;
};

xBinaryReader.prototype.readPoint = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var coords = this.readFloat32(count * 3);
    var result = new Array(count);
    for (var i = 0; i < count; i++) {
        var offset = i * 3 * 4;
        //only create new view on the buffer so that no new memory is allocated
        var point = new Float32Array(coords.buffer, offset, 3);
        result[i] = point;
    }
    return count === 1 ? result[0] : result;
};
xBinaryReader.prototype.readRgba = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var values = this.readByte(count * 4);
    var result = new Array(count);
    for (var i = 0; i < count ; i++) {
        var offset = i * 4;
        var colour = new Uint8Array(values.buffer, offset, 4);
        result[i] = colour;
    }
    return count === 1 ? result[0] : result;
};
xBinaryReader.prototype.readPackedNormal = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var values = this.readUint8(count * 2);
    var result = new Array(count);
    for (var i = 0; i < count; i++) {
        var uv = new Uint8Array(values.buffer, i * 2, 2);
        result[i] = uv;
    }
    return count === 1 ? result[0] : result;
};
xBinaryReader.prototype.readMatrix4x4 = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var values = this.readFloat32(count * 16);
    var result = new Array(count);
    for (var i = 0; i < count; i++) {
        var offset = i * 16 * 4;
        var matrix = new Float32Array(values.buffer, offset, 16);
        result[i] = matrix;
    }
    return count === 1 ? result[0] : result;
};
xBinaryReader.prototype.readMatrix4x4_64 = function (count) {
    if (typeof (count) === "undefined") count = 1;
    var values = this.readFloat64(count * 16);
    var result = new Array(count);
    for (var i = 0; i < count; i++) {
        var offset = i * 16 * 8;
        var matrix = new Float64Array(values.buffer, offset, 16);
        result[i] = matrix;
    }
    return count === 1 ? result[0] : result;
};function xModelGeometry() {
    //all this data is to be fed into GPU as attributes
    this.normals = [];
    this.indices = [];
    this.products = [];
    this.transformations = [];
    this.styleIndices = [];
    this.states = []; //this is the only array we need to keep alive on client side to be able to change appearance of the model

    //these will be sent to GPU as the textures
    this.vertices = [];
    this.matrices = [];
    this.styles = [];

    this.meter = 1000;

    //this will be used to change appearance of the objects
    //map objects have a format: 
    //map = {
    //	productID: int,
    //	type: int,
    //	bBox: Float32Array(6),
    //	spans: [Int32Array([int, int]),Int32Array([int, int]), ...] //spanning indexes defining shapes of product and it's state
    //};

    this.productMap = {};
}

xModelGeometry.prototype.parse = function (binReader) {
    var br = binReader;
    var magicNumber = br.readInt32();
    if (magicNumber != 94132117) throw 'Magic number mismatch.';
    var version = br.readByte();
    var numShapes = br.readInt32();
    var numVertices = br.readInt32();
    var numTriangles = br.readInt32();
    var numMatrices = br.readInt32();;
    var numProducts = br.readInt32();;
    var numStyles = br.readInt32();;
    this.meter = br.readFloat32();;
    var numRegions = br.readInt16();



    //set size of arrays to be square usable for texture data
    //TODO: reflect support for floating point textures
    var square = function (arity, count) {
        if (typeof (arity) == 'undefined' || typeof (count) == 'undefined') {
            throw 'Wrong arguments';
        }
        if (count == 0) return 0;
        var byteLength = count * arity;
        var imgSide = Math.ceil(Math.sqrt(byteLength / 4));
        //clamp to parity
        while ((imgSide * 4) % arity != 0) {
            imgSide++
        }
        var result = imgSide * imgSide * 4 / arity;
        return result;
    };

    //create target buffers of correct size (avoid reallocation of memory)
    this.vertices = new Float32Array(square(4, numVertices * 3));
    this.normals = new Uint8Array(numTriangles * 6);
    this.indices = new Float32Array(numTriangles * 3);
    this.styleIndices = new Uint16Array(numTriangles * 3);
    this.styles = new Uint8Array(square(1, (numStyles + 1) * 4)); //+1 is for a default style
    this.products = new Float32Array(numTriangles * 3);
    this.states = new Uint8Array(numTriangles * 3 * 2); //place for state and restyling
    this.transformations = new Float32Array(numTriangles * 3);
    this.matrices = new Float32Array(square(4, numMatrices * 16));
    this.productMap = {};
    this.regions = new Array(numRegions);

    var iVertex = 0;
    var iIndexForward = 0;
    var iIndexBackward = numTriangles * 3;
    var iTransform = 0;
    var iMatrix = 0;

    var stateEnum = xState;
    var typeEnum = xProductType;


    for (var i = 0; i < numRegions; i++) {
        this.regions[i] = {
            population: br.readInt32(),
            centre: br.readFloat32(3),
            bbox: br.readFloat32(6)
        }
    }


    var styleMap = [];
    styleMap.getStyle = function(id) {
        for (var i = 0; i < this.length; i++) {
            var item = this[i];
            if (item.id == id) return item;
        }
        return null;
    };
    var iStyle = 0;
    for (iStyle; iStyle < numStyles; iStyle++) {
        var styleId = br.readInt32();
        var R = br.readFloat32() * 255;
        var G = br.readFloat32() * 255;
        var B = br.readFloat32() * 255;
        var A = br.readFloat32() * 255;
        this.styles.set([R, G, B, A], iStyle * 4);
        styleMap.push({ id: styleId, index: iStyle, transparent: A < 254 });
    }
    this.styles.set([255, 255, 255, 255], iStyle * 4);
    var defaultStyle = { id: -1, index: iStyle, transparent: A < 254 }
    styleMap.push(defaultStyle);

    for (var i = 0; i < numProducts ; i++) {
        var productLabel = br.readInt32();
        var prodType = br.readInt16();
        var bBox = br.readFloat32(6);

        var map = {
            productID: productLabel,
            type: prodType,
            bBox: bBox,
            spans: []
        };
        this.productMap[productLabel] = map;
    }

    for (var iShape = 0; iShape < numShapes; iShape++) {

        var repetition = br.readInt32();
        var shapeList = [];
        for (var iProduct = 0; iProduct < repetition; iProduct++) {
            var prodLabel = br.readInt32();
            var instanceTypeId = br.readInt16();
            var instanceLabel = br.readInt32();
            var styleId = br.readInt32();
            var transformation = null;

            if (repetition > 1) {
                transformation = version === 1 ? br.readFloat32(16) : br.readFloat64(16);
                this.matrices.set(transformation, iMatrix);
                iMatrix += 16;
            }

            var styleItem = styleMap.getStyle(styleId);
            if (styleItem === null)
                styleItem = defaultStyle;

            shapeList.push({
                pLabel: prodLabel,
                iLabel: instanceLabel,
                style: styleItem.index,
                transparent: styleItem.transparent,
                transform: transformation != null ? iTransform++ : 0xFFFF
            });
        }

        //read shape geometry
        var shapeGeom = new xTriangulatedShape();
        shapeGeom.parse(br);


        //copy shape data into inner array and set to null so it can be garbage collected
        shapeList.forEach(function (shape) {
            var iIndex = 0;
            //set iIndex according to transparency either from beginning or at the end
            if (shape.transparent) {
                iIndex = iIndexBackward - shapeGeom.indices.length;
            }
            else {
                iIndex = iIndexForward;
            }

            var begin = iIndex;
            var map = this.productMap[shape.pLabel];
            if (typeof (map) === "undefined") {
                //throw "Product hasn't been defined before.";
                map = {
                    productID: 0,
                    type: typeEnum.IFCOPENINGELEMENT,
                    bBox: new Float32Array(6),
                    spans: []
                };
                this.productMap[shape.pLabel] = map;
            }

            this.normals.set(shapeGeom.normals, iIndex * 2);

            //switch spaces and openings off by default 
            var state = map.type == typeEnum.IFCSPACE || map.type == typeEnum.IFCOPENINGELEMENT ?
                stateEnum.HIDDEN :
                0xFF; //0xFF is for the default state

            //fix indices to right absolute position. It is relative to the shape.
            for (var i = 0; i < shapeGeom.indices.length; i++) {
                this.indices[iIndex] = shapeGeom.indices[i] + iVertex / 3;
                this.products[iIndex] = shape.pLabel;
                this.styleIndices[iIndex] = shape.style;
                this.transformations[iIndex] = shape.transform;
                this.states[2 * iIndex] = state; //set state
                this.states[2 * iIndex + 1] = 0xFF; //default style

                iIndex++;
            }

            var end = iIndex;
            map.spans.push(new Int32Array([begin, end]));

            if (shape.transparent) iIndexBackward -= shapeGeom.indices.length;
            else iIndexForward += shapeGeom.indices.length;
        }, this);

        //copy geometry and keep track of amount so that we can fix indices to right position
        //this must be the last step to have correct iVertex number above
        this.vertices.set(shapeGeom.vertices, iVertex);
        iVertex += shapeGeom.vertices.length;
        shapeGeom = null;
    }

    //binary reader should be at the end by now
    if (!br.getIsEOF()) {
        //throw 'Binary reader is not at the end of the file.';
    }

    this.transparentIndex = iIndexForward;
};

//Source has to be either URL of wexBIM file or Blob representing wexBIM file
xModelGeometry.prototype.load = function (source) {
    //binary reading
    var br = new xBinaryReader();
    var self = this;
    br.onloaded = function () {
        self.parse(br);
        if (self.onloaded) {
            self.onloaded();
        }
    };
    br.onerror = function (msg) {
        if (self.onerror) self.onerror(msg);
    };
    br.load(source);
};

xModelGeometry.prototype.onloaded = function () { };
xModelGeometry.prototype.onerror = function () { };function xTriangulatedShape() { };

//this will get xBinaryReader on the current position and will parse it's content to fill itself with vertices, normals and vertex indices
xTriangulatedShape.prototype.parse = function (binReader) {
    var self = this;
    var version = binReader.readByte();
    var numVertices = binReader.readInt32();
    var numOfTriangles = binReader.readInt32();
    self.vertices = binReader.readFloat32(numVertices * 3);
    //allocate memory of defined size (to avoid reallocation of memory)
    self.indices = new Uint32Array(numOfTriangles * 3);
    self.normals = new Uint8Array(numOfTriangles * 6);
    //indices for incremental adding of indices and normals
    var iIndex = 0;
    var readIndex;
    if (numVertices <= 0xFF) {
        readIndex = function (count) { return binReader.readByte(count); };
    }
    else if (numVertices <= 0xFFFF) {
        readIndex = function (count) { return binReader.readUint16(count); };
    }
    else {
        readIndex = function (count) { return binReader.readInt32(count); };
    }
    
    var numFaces = binReader.readInt32();

    if (numVertices === 0 || numOfTriangles === 0)
        return;

    for (var i = 0; i < numFaces; i++) {
        var numTrianglesInFace = binReader.readInt32();
        if (numTrianglesInFace == 0) continue;

        var isPlanar = numTrianglesInFace > 0;
        numTrianglesInFace = Math.abs(numTrianglesInFace);
        if (isPlanar) {
            var normal = binReader.readByte(2);
            //read and set all indices
            var planarIndices = readIndex(3 * numTrianglesInFace);
            self.indices.set(planarIndices, iIndex);

            for (var j = 0; j < numTrianglesInFace*3; j++) {
                //add three identical normals because this is planar but needs to be expanded for WebGL
                self.normals[iIndex * 2] = normal[0];
                self.normals[iIndex * 2 + 1] = normal[1];
                iIndex++;
            }
        }
        else {
            for (var j = 0; j < numTrianglesInFace; j++) {
                self.indices[iIndex] = readIndex();//a
                self.normals.set(binReader.readByte(2), iIndex * 2);
                iIndex++;

                self.indices[iIndex] = readIndex();//b
                self.normals.set(binReader.readByte(2), iIndex * 2);
                iIndex++;

                self.indices[iIndex] = readIndex();//c
                self.normals.set(binReader.readByte(2), iIndex * 2);
                iIndex++;
            }
        }
    }
};

//This would load only shape data from binary file
xTriangulatedShape.prototype.load = function (source) {
    //binary reading
    var br = new xBinaryReader();
    var self = this;
    br.onloaded = function () {
        self.parse(br);
        if (self.onloaded) {
            self.onloaded();
        }
    };
    br.load(source);
};


xTriangulatedShape.prototype.vertices = [];
xTriangulatedShape.prototype.indices = [];
xTriangulatedShape.prototype.normals = [];

//this function will get called when loading is finished.
//This won't get called after parse which is supposed to happen in large operation.
xTriangulatedShape.prototype.onloaded = function () { };

/**
    * Enumeration for object states.
    * @readonly
    * @enum {number}
    */
var xState = {
    UNDEFINED: 255,
    HIDDEN: 254,
    HIGHLIGHTED: 253,
    XRAYVISIBLE: 252,
    UNSTYLED: 225
};

/**
* Enumeration of product types.
* @readonly
* @enum {number}
*/
var xProductType = {

    IFCDISTRIBUTIONELEMENT: 44,
    IFCDISTRIBUTIONFLOWELEMENT: 45,
    IFCDISTRIBUTIONCHAMBERELEMENT: 180,
    IFCENERGYCONVERSIONDEVICE: 175,
    IFCAIRTOAIRHEATRECOVERY: 1097,
    IFCBOILER: 1105,
    IFCBURNER: 1109,
    IFCCHILLER: 1119,
    IFCCOIL: 1124,
    IFCCONDENSER: 1132,
    IFCCOOLEDBEAM: 1141,
    IFCCOOLINGTOWER: 1142,
    IFCENGINE: 1164,
    IFCEVAPORATIVECOOLER: 1166,
    IFCEVAPORATOR: 1167,
    IFCHEATEXCHANGER: 1187,
    IFCHUMIDIFIER: 1188,
    IFCTUBEBUNDLE: 1305,
    IFCUNITARYEQUIPMENT: 1310,
    IFCELECTRICGENERATOR: 1160,
    IFCELECTRICMOTOR: 1161,
    IFCMOTORCONNECTION: 1216,
    IFCSOLARDEVICE: 1270,
    IFCTRANSFORMER: 1303,
    IFCFLOWCONTROLLER: 121,
    IFCELECTRICDISTRIBUTIONPOINT: 242,
    IFCAIRTERMINALBOX: 1096,
    IFCDAMPER: 1148,
    IFCFLOWMETER: 1182,
    IFCVALVE: 1311,
    IFCELECTRICDISTRIBUTIONBOARD: 1157,
    IFCELECTRICTIMECONTROL: 1162,
    IFCPROTECTIVEDEVICE: 1235,
    IFCSWITCHINGDEVICE: 1290,
    IFCFLOWFITTING: 467,
    IFCDUCTFITTING: 1153,
    IFCPIPEFITTING: 1222,
    IFCCABLECARRIERFITTING: 1111,
    IFCCABLEFITTING: 1113,
    IFCJUNCTIONBOX: 1195,
    IFCFLOWMOVINGDEVICE: 502,
    IFCCOMPRESSOR: 1131,
    IFCFAN: 1177,
    IFCPUMP: 1238,
    IFCFLOWSEGMENT: 574,
    IFCDUCTSEGMENT: 1154,
    IFCPIPESEGMENT: 1223,
    IFCCABLECARRIERSEGMENT: 1112,
    IFCCABLESEGMENT: 1115,
    IFCFLOWSTORAGEDEVICE: 371,
    IFCTANK: 1293,
    IFCELECTRICFLOWSTORAGEDEVICE: 1159,
    IFCFLOWTERMINAL: 46,
    IFCFIRESUPPRESSIONTERMINAL: 1179,
    IFCSANITARYTERMINAL: 1262,
    IFCSTACKTERMINAL: 1277,
    IFCWASTETERMINAL: 1315,
    IFCAIRTERMINAL: 1095,
    IFCMEDICALDEVICE: 1212,
    IFCSPACEHEATER: 1272,
    IFCAUDIOVISUALAPPLIANCE: 1099,
    IFCCOMMUNICATIONSAPPLIANCE: 1127,
    IFCELECTRICAPPLIANCE: 1156,
    IFCLAMP: 1198,
    IFCLIGHTFIXTURE: 1199,
    IFCOUTLET: 1219,
    IFCFLOWTREATMENTDEVICE: 425,
    IFCINTERCEPTOR: 1193,
    IFCDUCTSILENCER: 1155,
    IFCFILTER: 1178,
    IFCDISTRIBUTIONCONTROLELEMENT: 468,
    IFCPROTECTIVEDEVICETRIPPINGUNIT: 1236,
    IFCACTUATOR: 1091,
    IFCALARM: 1098,
    IFCCONTROLLER: 1139,
    IFCFLOWINSTRUMENT: 1181,
    IFCSENSOR: 1264,
    IFCUNITARYCONTROLELEMENT: 1308,
    IFCDISCRETEACCESSORY: 423,
    IFCFASTENER: 535,
    IFCMECHANICALFASTENER: 536,
    IFCREINFORCINGBAR: 571,
    IFCREINFORCINGMESH: 531,
    IFCTENDON: 261,
    IFCTENDONANCHOR: 675,
    IFCBUILDINGELEMENTPART: 220,
    IFCMECHANICALFASTENER: 536,
    IFCVIBRATIONISOLATOR: 1312,
    IFCCHAMFEREDGEFEATURE: 765,
    IFCROUNDEDEDGEFEATURE: 766,
    IFCOPENINGELEMENT: 498,
    IFCOPENINGSTANDARDCASE: 1217,
    IFCVOIDINGFEATURE: 1313,
    IFCPROJECTIONELEMENT: 384,
    IFCSURFACEFEATURE: 1287,
    IFCBUILDINGELEMENTPART: 220,
    IFCREINFORCINGBAR: 571,
    IFCREINFORCINGMESH: 531,
    IFCTENDON: 261,
    IFCTENDONANCHOR: 675,
    IFCFOOTING: 120,
    IFCPILE: 572,
    IFCBEAM: 171,
    IFCBEAMSTANDARDCASE: 1104,
    IFCCOLUMN: 383,
    IFCCOLUMNSTANDARDCASE: 1126,
    IFCCURTAINWALL: 456,
    IFCDOOR: 213,
    IFCDOORSTANDARDCASE: 1151,
    IFCMEMBER: 310,
    IFCMEMBERSTANDARDCASE: 1214,
    IFCPLATE: 351,
    IFCPLATESTANDARDCASE: 1224,
    IFCRAILING: 350,
    IFCRAMP: 414,
    IFCRAMPFLIGHT: 348,
    IFCROOF: 347,
    IFCSLAB: 99,
    IFCSLABELEMENTEDCASE: 1268,
    IFCSLABSTANDARDCASE: 1269,
    IFCSTAIR: 346,
    IFCSTAIRFLIGHT: 25,
    IFCWALL: 452,
    IFCWALLSTANDARDCASE: 453,
    IFCWALLELEMENTEDCASE: 1314,
    IFCWINDOW: 667,
    IFCWINDOWSTANDARDCASE: 1316,
    IFCBUILDINGELEMENTPROXY: 560,
    IFCCOVERING: 382,
    IFCCHIMNEY: 1120,
    IFCSHADINGDEVICE: 1265,
    IFCELEMENTASSEMBLY: 18,
    IFCFURNISHINGELEMENT: 253,
    IFCFURNITURE: 1184,
    IFCSYSTEMFURNITUREELEMENT: 1291,
    IFCTRANSPORTELEMENT: 416,
    IFCVIRTUALELEMENT: 168,
    IFCELECTRICALELEMENT: 23,
    IFCEQUIPMENTELEMENT: 212,
    IFCCIVILELEMENT: 1122,
    IFCGEOGRAPHICELEMENT: 1185,
    IFCDISTRIBUTIONPORT: 178,
    IFCPROXY: 447,
    IFCSTRUCTURALLINEARACTION: 463,
    IFCSTRUCTURALLINEARACTIONVARYING: 464,
    IFCSTRUCTURALPLANARACTION: 39,
    IFCSTRUCTURALPLANARACTIONVARYING: 357,
    IFCSTRUCTURALPOINTACTION: 356,
    IFCSTRUCTURALCURVEACTION: 1279,
    IFCSTRUCTURALLINEARACTION: 463,
    IFCSTRUCTURALSURFACEACTION: 1284,
    IFCSTRUCTURALPLANARACTION: 39,
    IFCSTRUCTURALPOINTREACTION: 354,
    IFCSTRUCTURALCURVEREACTION: 1280,
    IFCSTRUCTURALSURFACEREACTION: 1285,
    IFCSTRUCTURALCURVECONNECTION: 534,
    IFCSTRUCTURALPOINTCONNECTION: 533,
    IFCSTRUCTURALSURFACECONNECTION: 264,
    IFCSTRUCTURALCURVEMEMBER: 224,
    IFCSTRUCTURALCURVEMEMBERVARYING: 227,
    IFCSTRUCTURALSURFACEMEMBER: 420,
    IFCSTRUCTURALSURFACEMEMBERVARYING: 421,
    IFCANNOTATION: 634,
    IFCBUILDING: 169,
    IFCBUILDINGSTOREY: 459,
    IFCSITE: 349,
    IFCSPACE: 454,
    IFCGRID: 564,
    IFCBUILDING: 169,
    IFCBUILDINGSTOREY: 459,
    IFCSITE: 349,
    IFCSPACE: 454,
    IFCEXTERNALSPATIALELEMENT: 1174,
    IFCSPATIALZONE: 1275
};
