/**
 * PDFAnnotate v1.0.0
 * Author: Ravisha Heshan
 */

var PDFAnnotate = function (container_id, url, options = {}) {
    this.number_of_pages = 0;
    this.pages_rendered = 0;
    this.active_tool = 1; // 1 - Free hand, 2 - Text, 3 - Arrow, 4 - Rectangle
    this.fabricObjects = [];
    this.fabricObjectsData = [];
    this.color = '#212121';
    this.borderColor = '#000000';
    this.borderSize = 1;
    this.font_size = 16;
    this.active_canvas = 0;
    this.container_id = container_id;
    this.url = url;
    var inst = this;
    var loadingTask = PDFJS.getDocument(this.url);
    loadingTask.promise.then(function (pdf) {
        var scale = 1.3;
        inst.number_of_pages = pdf.pdfInfo.numPages;
        for (var i = 1; i <= pdf.pdfInfo.numPages; i++) {
            pdf.getPage(i).then(function (page) {
                var viewport = page.getViewport(scale);
                var canvas = document.createElement('canvas');
                document.getElementById(inst.container_id).appendChild(canvas);
                canvas.className = 'pdf-canvas';
                canvas.height = viewport.height;
                canvas.width = viewport.width;
                context = canvas.getContext('2d');
                var renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                var renderTask = page.render(renderContext);
                renderTask.then(function () {
                    $('.pdf-canvas').each(function (index, el) {
                        $(el).attr('id', 'page-' + (index + 1) + '-canvas');
                    });
                    inst.pages_rendered++;
                    if (inst.pages_rendered == inst.number_of_pages) inst.initFabric();
                });
            });
        }
    }, function (reason) {
    });

    this.initFabric = function () {
        var inst = this;
        let canvases = $('#' + inst.container_id + ' canvas')
        canvases.each(function (index, el) {
            var background = el.toDataURL("image/png");
            var fabricObj = new fabric.Canvas(el.id, {
                freeDrawingBrush: {
                    width: 1,
                    color: inst.color
                }
            });
            inst.fabricObjects.push(fabricObj);
            if (typeof options.onPageUpdated == 'function') {
                fabricObj.on('object:added', function () {
                    var oldValue = Object.assign({}, inst.fabricObjectsData[index]);
                    inst.fabricObjectsData[index] = fabricObj.toJSON()
                    options.onPageUpdated(index + 1, oldValue, inst.fabricObjectsData[index])
                })
            }
            fabricObj.setBackgroundImage(background, fabricObj.renderAll.bind(fabricObj));
            $(fabricObj.upperCanvasEl).click(function (event) {
                inst.active_canvas = index;
                inst.fabricClickHandler(event, fabricObj);
            });
            fabricObj.on('after:render', function () {
                inst.fabricObjectsData[index] = fabricObj.toJSON()
                fabricObj.off('after:render')
            })
            if (index === canvases.length - 1 && typeof options.ready === 'function') {
                options.ready()
            }
        });
    }

    this.fabricClickHandler = function (event, fabricObj) {
        if (fabricObj._hoveredTarget != null) {
            if (existeMarco == true) {
                $("#IdX").val(fabricObj._hoveredTarget.oCoords.tl.x);
                $("#IdY").val(fabricObj._hoveredTarget.oCoords.tl.y);
                $("#PaginaActual").val(PaginaActual);
                console.log(fabricObj._hoveredTarget.oCoords);
            }
        }
    }
}

PDFAnnotate.prototype.enableRectangle = function (x, y) {
    var inst = this;
    var fabricObj = inst.fabricObjects[inst.active_canvas];
    PaginaActual = inst.active_canvas + 1;
    inst.active_tool = 4;
    if (inst.fabricObjects.length > 0) {
        $.each(inst.fabricObjects, function (index, fabricObj) {
            fabricObj.isDrawingMode = false;
        });
    }
    var rect = new fabric.Rect({
        left: x,
        top: y,
        width: 50,
        height: 50,
        fill: inst.color,
        stroke: inst.borderColor,
        strokeSize: inst.borderSize
    });
    fabricObj.add(rect);
    rect.setControlsVisibility({
        tl: false,
        tr: false,
        bl: false,
        br: false,
        mtr: false,
        mb: false,
        ml: false,
        mr: false,
        mt: false,
    });
}

PDFAnnotate.prototype.deleteSelectedObject = function () {
    var inst = this;
    var activeObject = inst.fabricObjects[inst.active_canvas].getActiveObject();
    if (activeObject) {
        inst.fabricObjects[inst.active_canvas].remove(activeObject);
        existeMarco = false;
    }
}

PDFAnnotate.prototype.setBrushSize = function (size) {
    var inst = this;
    $.each(inst.fabricObjects, function (index, fabricObj) {
        fabricObj.freeDrawingBrush.width = size;
    });
}

PDFAnnotate.prototype.setColor = function (color) {
    var inst = this;
    inst.color = color;
    $.each(inst.fabricObjects, function (index, fabricObj) {
        fabricObj.freeDrawingBrush.color = color;
    });
}

PDFAnnotate.prototype.setBorderColor = function (color) {
    var inst = this;
    inst.borderColor = color;
}

PDFAnnotate.prototype.serializePdf = function () {
    var inst = this;
    return JSON.stringify(inst.fabricObjects, null, 4);
}

PDFAnnotate.prototype.clearActivePage = function () {
    var inst = this;
    var fabricObj = inst.fabricObjects[inst.active_canvas];
    var bg = fabricObj.backgroundImage;
    fabricObj.clear();
    fabricObj.setBackgroundImage(bg, fabricObj.renderAll.bind(fabricObj));
    existeMarco = false;
}

PDFAnnotate.prototype.loadFromJSON = function (jsonData) {
    var inst = this;
    $.each(inst.fabricObjects, function (index, fabricObj) {
        if (jsonData.length > index) {
            fabricObj.loadFromJSON(jsonData[index], function () {
                inst.fabricObjectsData[index] = fabricObj.toJSON()
            })
        }
    })
}