var ControlLink = false;

(function () {
    this.App = {
        setCalendarLenguage: function () {
            $.datepicker.regional['es'] = {
                closeText: 'Cerrar',
                prevText: '< Ant',
                nextText: 'Sig >',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: '',
                autoclose: true
            };
            return $.datepicker.setDefaults($.datepicker.regional['es']);
        },
        propertiesCalendar: {
            showOn: "button",
            buttonImage: "../img/calendar.png",
            buttonImageOnly: true,
            buttonText: "Select date"
        },
        createDatePickerInput: function (ob) {
            var _this;
            _this = this;
            _this.setCalendarLenguage();
            return $.each(ob, function (key, value) {
                if ($(value).data("display-button")) {
                    return $(value).datepicker(_this.propertiesCalendar);
                } else {
                    return $(value).datepicker();
                }
            });
        },
        LoaderShow: function () {
            if ($('body').hasClass("loaded")) {
                $('body').removeClass('loaded');
                window.parent.postMessage("alertOpen", "*");
            }
        },
        LoaderHide: function () {
            if (!$('body').hasClass("loaded")) {
                $('body').addClass('loaded');
                window.parent.postMessage("alertClose", "*");
            }
        },
        MsjAjaxSuccces: function (msj) {
            alertBase('success', '¡Listo!', msj, null);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxSucccesWithTimer: function (msj, fn_Ejecutar) {
            alertBase('success', '¡Listo!', msj, null, 5000);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxSuccessWithLocation: function (msj, fn_Ejecutar) {
            alertBase('success', '¡Listo!', msj, fn_Ejecutar);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxSuccessWithFunctionAndTimer: function (msj, fn_Ejecutar) {
            alertBase('success', '¡Listo!', msj, fn_Ejecutar, 5000);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxConfirmationWithFunction: function (msj, fn_Ejecutar) {
            alertBase('confirm', '¡Confirmar!', msj, function (ans) {
                if (ans) {
                    fn_Ejecutar();
                }
            });
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxConfirmationWithFunctionAndParameters: function (msj, fn_Ejecutar, param, param2, param3, param4) {
            alertBase('confirm', '¡Confirmar!', msj, function (ans) {
                if (ans) {
                    fn_Ejecutar(param, param2, param3, param4);
                }
            });
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxConfirmationWithFunctionOK_CancelAndParameters: function (msj, fn_Ejecutar, fn_Cancel, param) {
            alertBase('confirm', '¡Confirmar!', msj, function (ans) {
                if (ans) {
                    fn_Ejecutar(param);
                }
            }, null, function (ansca) {
                if (ansca) {
                    fn_Cancel(param);
                }
            },
            );
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxConfirmationWithLocation: function (msj, location) {
            alertBase('confirm', '¡Confirmar!', msj, function (ans) {
                if (ans) {
                    window.parent.location.replace(location);
                }
            });
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxError: function () {
            alertBase('error', '¡Error!', "Ocurrió un error, por favor comuníquese con el administrador del sistema.");
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxErrorCustom: function (msj) {
            alertBase('error', '¡Error!', msj);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxWarning: function (msj) {
            alertBase('warning', '¡Alerta!', msj);
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxWarningWithFunction: function (msj, fn_Ejecutar) {
            alertBase('warning', '¡Alerta!', msj, function (ans) {
                if (ans) {
                    fn_Ejecutar();
                }
            });
            window.parent.postMessage("alertOpen", "*");
        },
        MsjAjaxRedirect: function (msj, fn_Ejecutar) {
            alertBase('redirect', '¡Alerta!', msj, function (ans) {
                if (ans) {
                    fn_Ejecutar();
                }
            });
            window.parent.postMessage("alertOpen", "*");
        }
    };
    this.Msj = {
        Registro: "Se registró correctamente",
        Actualizo: "Se Actualizó correctamente",
        Elimino: "Se eliminó correctamente",
        ConfirmaGuardar: "¿Está seguro de guardar el registro?",
        EliminarRegistro: "¿Está seguro de eliminar el registro?",
        EliminarRegistroConDependencias: "¿Está seguro de eliminar el registro, ya que cuenta con registros relacionados?",
        EliminarDocumento: "¿Está seguro de eliminar el documento?",
        CancelarRegistro: "¿Está seguro de cancelar?",
        ConfirmaAprobar: "¿Está seguro de aprobar el acta?",
        ElegirAlMenosUno: "Debe elegir al menos {0}",
        DocPublicado: "un documento publicado",
        DesactivarRegistro: "¿Está seguro de desactivar el registro?",
        ActivarRegistro: "¿Está seguro de activar el registro?",
        SolicitudObservar: "¿Está seguro de observar el documento?",
        SolicitudAprobar: "¿Está seguro de aprobar el documento?",
        CampoObligatorio: "Campo {0} obligatorio",
        CampoSoloNumero: "Campo {0} solo permite numeros"
    };
    this.CodigoEstadoDocumento = {
        CREAR: "CREAR",
        VALIDAR: "VALIDAR",
        REVISAR: "REVISAR",
        APROBAR: "APROBAR",
        RETORNO: "RETORNO",
    };
    this.CodigoSolicitudDocumento = {
        CREACION: "CRE",
        ACTUALIZAR: "ACT",
        OBSOLESCENCIA: "OBS",
    };
}).call(this);

$.ajaxSetup({
    beforeSend: function () {
        App.LoaderShow();
    },
    complete: function () {
        App.LoaderHide();
    },
    error: function (jqXHR, textStatus, errorThrown) {
        App.LoaderHide();
        var html = $($.parseHTML(jqXHR.responseText));
        var title = $.parseHTML(html.filter('title').html());
        if (jqXHR.status === 0) {
            App.MsjAjaxErrorCustom('No conecta: ​​verificar red. Actualice su interfaz.');
        } else if (jqXHR.status == 404) {
            App.MsjAjaxErrorCustom('Página solicitada no encontrada [404]. Actualice su interfaz.');
        } else if (jqXHR.status == 500) {
            App.MsjAjaxErrorCustom('Error interno del servidor [500]. Actualice su interfaz.');
        } else if (textStatus === 'parsererror') {
            App.MsjAjaxErrorCustom('No se pudo obtener respuesta del servidor. Actualice su interfaz.');
        } else if (textStatus === 'timeout') {
            App.MsjAjaxErrorCustom('Error de tiempo de espera. Actualice su interfaz.');
        } else if (textStatus === 'abort') {
            App.MsjAjaxErrorCustom('Petición abortada. Actualice su interfaz.');
        } else {
            App.MsjAjaxError();
        }
    }
});

function ControlFechasInicioFin(divDesde, calDesde, hdfDesde, divHasta, calHasta, hdfHasta) {
    $("#" + calDesde).change(function () {
        if ($(this).val() == "") {
            $("." + divHasta).datepicker("setStartDate", "");
        }
        if ($("#" + calHasta).val() == "") {
            $("." + divDesde).datepicker("setEndDate", "");
        }
        ChangeDateFormat('#' + calDesde, '#' + hdfDesde);
    });
    $("#" + calHasta).change(function () {
        if ($(this).val() == "") {
            $("." + divDesde).datepicker("setEndDate", "");
        }
        if ($("#" + calDesde).val() == "") {
            $("." + divHasta).datepicker("setStartDate", "");
        }
        ChangeDateFormat('#' + calHasta, '#' + hdfHasta);
    });
    $("." + divDesde).datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        orientation: "auto",
        minDate: "12/09/2018"
    }).on('changeDate', function (e) {
        $("." + divHasta).datepicker("setStartDate", (e.format(0, "dd/mm/yyyy")));
    });
    $("." + divHasta).datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        orientation: "auto"
    }).on('changeDate', function (e) {
        $("." + divDesde).datepicker("setEndDate", (e.format(0, "dd/mm/yyyy")));
    });
    if ($("#" + calDesde).val() != "") {
        $("." + divHasta).datepicker("setStartDate", $("#" + calDesde).val());
    }
    if ($("#" + calHasta).val() != "") {
        $("." + divDesde).datepicker("setEndDate", $("#" + calHasta).val());
    }
}

function ChangeDateFormat(IdDate, HiddenDate) {
    let val = $(IdDate).val();
    if (val != "") {
        let datePart = val.split("/")
        val = (datePart[1] + "/" + datePart[0] + "/" + datePart[2]);
    }
    $(HiddenDate).val(val);
}

function parseElement(val) {
    if (val != undefined && val != null && val != "" && val != "-1") {
        if (!$.isNumeric(val)) {
            return val.trim().replace(/&/g, "&amp;");
        } else {
            return val;
        }
    }
    return null;
}

function ChangeInputFile(input) {
    if (input != undefined) {
        var divid = $(input).parent().attr("id");
        $("#" + divid + " label").html($(input).val().split("\\").pop())
    }
}

function getQueryStringParameter(urlParameterKey) {
    var params = document.URL.split('?')[1].split('&');
    var strParams = '';
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split('=');
        if (singleParam[0] === urlParameterKey) {
            return singleParam[1];
        }
    }
}

function createPeoplePicker(context, divContener, control, divControlBusqueda, controlHdn, spLanguage, instancia, MaxUsers, ServerDataMethod, GrupoName) {
    var newPicker;
    newPicker = new CAMControl.PeoplePicker(context, divContener, control, divControlBusqueda, controlHdn, ServerDataMethod);
    newPicker.InstanceName = instancia;
    newPicker.Language = spLanguage;
    newPicker.MaxEntriesShown = 3;
    newPicker.AllowDuplicates = false;
    newPicker.ShowLoginName = true;
    newPicker.ShowTitle = true;
    newPicker.PrincipalType = 1;
    newPicker.MaxUsers = MaxUsers;
    newPicker.MinimalCharactersBeforeSearching = 4;
    newPicker.Initialize();
    return newPicker;
}

function ObtenerListaUsuarioToPeoplePicker(val) {
    var Resultado = [];
    if (val != undefined && val != null && val != "" && val != "[]") {
        var json = JSON.parse(val);
        if (json.length > 0) {
            for (var i = 0; i < json.length; i = i + 1) {
                if (json[i].Email != undefined && json[i].Email != null && json[i].Email != "") {
                    Resultado.push({ Correo: json[i].Email, Nombre: json[i].Name });
                }
            }
            return Resultado;
        }
    }
    return Resultado;
}

function ObtenerListaUsuarioToString(val) {
    var Resultado = "";
    if (val != undefined && val != null && val != "" && val != "[]") {
        var json = JSON.parse(val);
        if (json.length > 0) {
            for (var i = 0; i < json.length; i = i + 1) {
                if (json[i].Name != undefined && json[i].Name != null && json[i].Name != "") {
                    Resultado = Resultado + json[i].Name + ";";
                }
            }
            return Resultado;
        }
    }
    return Resultado;
}

function ObtenerUsuarioToPeoplePicker(val) {
    if (val != undefined && val != null && val != "") {
        var json = JSON.parse(val);
        if (json.length > 0) {
            return json[0].Name;
        }
    }
    return null;
}

function getQuerystring(key) {
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null) {
        return null;
    } else {
        return qs[1];
    }
}

function WindowParentLocationReplace(location) {
    window.parent.location.replace(location);
}

function WindowOpenNewTab(location) {
    window.open(location, "_blank");
}

function ValidarFormulario(form) {
    var MsjObligatorio = "<small class='form-text text-muted field-validation-error'>" + Msj.CampoObligatorio + "</small>";
    var validacion = true;
    $(".form-text").remove();
    $("#" + form + " .form-control").removeClass("hasError");
    $("#" + form + " .custom-file").removeClass("hasError");
    $("#" + form + " div").removeClass("hasError");
    $("#" + form + " .select2-selection").removeClass("hasError");
    $("#" + form + " .custom-file-label").removeClass("hasError");
    $("#" + form + " .cam-peoplepicker-userlookup").removeClass("hasError");
    $("#" + form + " input").removeClass("hasErrorCheckBox");
    $("#" + form + " div").removeClass("is-invalid");
    $("#" + form + " .required").each(function () {
        if ($(this).is(":visible")) {
            if (this.type == "checkbox") {
                if (!$(this).prop('checked')) {
                    $(this).addClass("hasErrorCheckBox");
                    validacion = false;
                }
            } else if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "[]") {
                    if ($(this).hasClass("reference")) {
                        var reference = this.className.split("ref_")[1];
                        var objreference = $("#" + reference);
                        if (objreference[0].type == "checkbox") {
                            if ($(this).hasClass("controlSiNo")) {
                                if (objreference.prop('checked')) {
                                    $(this.parentElement).addClass("hasError");
                                    validacion = false;
                                }
                            } else {
                                if (!objreference.prop('checked')) {
                                    $(this).addClass("hasError");
                                    validacion = false;
                                }
                            }
                        } else {
                            objreference.addClass("hasError");
                            validacion = false;
                        }
                    } else {
                        if ($(this).hasClass("selectpicker")) {
                            $(this).parent().addClass("is-invalid");
                            var Label = $("label[for='" + $(this).attr("id") + "']").text();
                            $(this).parent().parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                            validacion = false;
                        } else if ($(this).hasClass("custom-file-input")) {
                            $(this).parent().find(".custom-file-label").addClass("hasError");
                            var Label = $("label[for='" + $(this).attr("id") + "']").text();
                            $(this).parent().parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                            validacion = false;
                        } else if ($(this).parent().hasClass("date")) {
                            $(this).addClass("hasError");
                            var Label = $("label[for='" + $(this).attr("id") + "']").text();
                            $(this).parent().parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                            validacion = false;
                        } else {
                            $(this).addClass("hasError");
                            var Label = $("label[for='" + $(this).attr("id") + "']").text();
                            $(this).parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                            validacion = false;
                        }
                    }
                } else {
                    if ($(this).attr("minlength") != undefined && $(this).attr("minlength") != null) {
                        if ($(this).val().length < $(this).attr("minlength")) {
                            $(this).addClass("hasError");
                            validacion = false;
                        }
                    }
                }
            } else if (this.localName == "p") {
                if ($(this).html() == "") {
                    $(this).addClass("hasError");
                    validacion = false;
                }
            }
        } else {
            if (this.type == "hidden") {
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "[]") {
                    if ($(this).hasClass("cam-peoplepicker-inputvalue")) {
                        $(this).parent().find(".cam-peoplepicker-userlookup").addClass("hasError");
                        var Label = $("label[for='" + $(this).attr("id") + "']").text();
                        $(this).parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                        validacion = false;
                    }
                }
            }
        }
    });
    Communica.Part.adjustSize();
    return validacion;
}

function formtBoolean(value, row, index) {
    if (value == true) {
        return "Si";
    } else {
        return "No";
    }
}

function getCookieFormulario(form) {
    $("#" + form + " .cookie").each(function () {
        var id = $(this).attr("Id");
        var value = Cookies.get(form + id);
        if (value != undefined && value != null && value != "") {
            $("#" + id).val(value.trim());
        }
    });
}

function setCookieFormulario(form) {
    $("#" + form + " .cookie").each(function () {
        var id = $(this).attr("Id");
        var value = $(this).val();
        if (value != undefined && value != null) {
            Cookies.set(form + id, value.trim());
        }
    });
}

function ConstruirDocumentoOnline(Link, SubSitio) {
    ControlLink = false;
    $(document).on("click", Link, function (e) {
        if (ControlLink == false) {
            ControlLink = true;
            setTimeout(function () { ControlLink = false; }, 3000);
            $(this).removeAttr("target");
            e.preventDefault();
            var UniqueId = $(this).attr("data-unique");
            var urlFile = $(this).attr("href");
            var siteUrl = urlFile.split(".com/");
            var urlTab = "";
            //urlTab = siteUrl[0] + ".com/:w:/r/" + siteUrl[1].split("/")[0] + "/" + siteUrl[1].split("/")[1] + "/" + siteUrl[1].split("/")[2] + "/_layouts/15/Doc.aspx?sourcedoc={" + UniqueId + "}&action=edit&source=" + siteUrl[0] + ".com/" + siteUrl[1].split("/")[0] + "/" + siteUrl[1].split("/")[1] + "/" + siteUrl[1].split("/")[2] + "/" + siteUrl[1].split("/")[3] + "/Forms/AllItems.aspx";
            if (SubSitio == true) {
                urlTab = siteUrl[0] + ".com/:w:/r/" + siteUrl[1].split("/")[0] + "/" + siteUrl[1].split("/")[1] + "/" + siteUrl[1].split("/")[2] + "/_layouts/15/Doc.aspx?sourcedoc=" + siteUrl[0] + ".com/" + siteUrl[1];
            } else {
                urlTab = siteUrl[0] + ".com/:w:/r/" + siteUrl[1].split("/")[0] + "/" + siteUrl[1].split("/")[1] + "/_layouts/15/Doc.aspx?sourcedoc=" + siteUrl[0] + ".com/" + siteUrl[1];
            }
            var win = window.open(urlTab);
            if (win) {
                win.focus();
            }
        }
    });
}

function opFormNro(value, row, index) {
    return index + 1;
}

function AsignarToolTip() {
    $('.selectpicker').on('show.bs.select', function (e, clickedIndex, isSelected, previousValue) {
        Communica.Part.adjustSize();
    });
    $('[data-toggle="dropdown"]').click(function (e) {
        setTimeout(function () { Communica.Part.adjustSize(); }, 5);
    });
    $(".date input").click(function (e) {
        setTimeout(function () { Communica.Part.adjustSize(); }, 5);
    });
}

function RegistrarExportar(InterfazId, EntidadId) {
    $.ajax({
        type: 'POST',
        url: UrlBase.RegistrarExportar,
        data: { InterfazId: InterfazId, EntidadId: EntidadId },
        async: true
    });
}

$(document).ready(function () {
    ControlLink = false;
    $(".close-drawer").on("click", function (e) {
        e.preventDefault();
        $(this).parent().parent().parent().parent().navdrawer('hide');
    });
    $(".tab").on("click", function (e) {
        setTimeout(function () { Communica.Part.adjustSize(); }, 250);
    });
});

window.addEventListener('message', function (event) {
    if (event.data == "notificaCambioPagina") {
        App.MsjAjaxConfirmationWithFunctionAndParameters('¿Está seguro de salir de la ventana, ya que perdera los cambios que no ha guardado?', RedirectParent, AppVolcan.Inicio);
    }
});

function VistaPreviaArchivos() {
    $("a.filePreview").focus(function () {
        $("#modalVistaPrevia .modal-body").empty();
        $("#modalVistaPrevia .modal-footer .text-right").empty();
        var href = $(this).attr("href");
        var fileName = href.replace(/^.*[\\\/]/, '');
        var recursoPreview = "Doc.aspx";
        if (fileName.indexOf(".pdf") != -1 || fileName.indexOf(".PDF") != -1) {
            recursoPreview = "WopiFrame.aspx";
        }
        var iframeFile = document.createElement('iframe');
        iframeFile.id = 'iframe-' + Math.floor((Math.random() * 100) + 1);
        iframeFile.src = SPHost + "/_layouts/15/" + recursoPreview + "?sourcedoc=" + href + "&action=interactivepreview&wdSmallView=1";
        iframeFile.scrolling = "no";
        iframeFile.frameborder = "0";
        iframeFile.marginheight = "0px";
        iframeFile.marginwidth = "0px";
        iframeFile.height = "400px";
        iframeFile.width = "100%";
        $("#modalVistaPrevia .modal-body").append(iframeFile);
        $(".tituloFile").html(fileName);
        var ahref = document.createElement('a');
        ahref.id = 'a-' + Math.floor((Math.random() * 100) + 1);
        ahref.href = href;
        ahref.className = "btn btn-jockey";
        ahref.target = "_blank";
        ahref.innerText = 'ABRIR';
        $("#btnAbrirDocumento").attr("href", href);
        $("#modalVistaPrevia .modal-footer .text-right").append(ahref);
        if (fileName.indexOf(".pdf") == -1 && fileName.indexOf(".PDF") == -1) {
            ConstruirDocumentoOnline("#" + ahref.id);
        }
        $("#modalVistaPrevia").modal("show");
    });
}

function ValidarCampoNumerico(Valor) {
    if ($.isNumeric(Valor)) {
        return Valor;
    }
    return 0;
}

function formattTextoSinHtml(value, row, index) {
    if (value != null) {
        return value.replace(/<[^>]*>?/g, '');
    } else {
        return "-";
    }
}

function setControlAuditoria(form) {
    $("#" + form + " .auditar").each(function () {
        if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
            var id = $(this).attr("Id");
            var value = $(this).val();
            Cookies.set(form + "auditar" + id, value);
        }
    });
}

function getControlAuditoria(form) {
    var Object = {};
    $("#" + form + " .auditar").each(function () {
        if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
            var id = $(this).attr("Id");
            var value = $(this).val();
            var valueCookie = Cookies.get(form + "auditar" + id);
            if (value != valueCookie) {
                var Label = "";
                if ($(this).hasClass("selectpicker")) {
                    Label = $(this).parent().parent().find("label:first").text();
                    value = $("#" + id + " option:selected").text();
                } else if ($(this).hasClass("custom-file-input")) {
                    Label = $(this).parent().parent().find("label:first").text();
                } else if ($(this).parent().hasClass("date")) {
                    Label = $(this).parent().parent().find("label:first").text();
                } else if ($(this).parent().hasClass("time_pick")) {
                    Label = $(this).parent().parent().parent().find("label:first").text();
                } else {
                    Label = $(this).parent().find("label:first").text();
                }
                Object[Label] = value;
            }
        }
    });
    return JSON.stringify(Object);
}

function formtFechaCreado(value, row, index, field) {
    if (value != null) {
        return row.CreadoFormato;
    } else {
        return "-";
    }
}

function formtFechaPublicacion(value, row, index, field) {
    if (value != null) {
        return row.FechaPublicacionFormato;
    } else {
        return "-";
    }
}

function DateSorter(a, b) {
    if (parseElement(a) != null && parseElement(b) != null) {
        var dateA = new Date(moment(a).format('YYYY-MM-DDTHH:mm:ss'));
        var dateB = new Date(moment(b).format('YYYY-MM-DDTHH:mm:ss'));
        return dateA - dateB;
    }
}

function Reload() {
    window.parent.postMessage("reload", "*");
}

function convertToUpper(obj) {
    if (obj != null && obj != "") {
        return obj.toUpperCase();
    }
}

function formatoUpperTable(value, row, index) {
    if (value != null || value != "") {
        return value.toUpperCase();
    }
}

function renderSPChrome() {
    var options = {
        'appTitle': document.title,
        'onCssLoaded': 'chromeLoaded()'
    }
};

function CargarControlesPeoplePicker(item, maxUsers, orden) {
    var layoutsRoot = ParametersApp.SPHost + '/_layouts/15/';
    $.getScript(layoutsRoot + 'SP.Runtime.js',
        function () {
            $.getScript(layoutsRoot + 'SP.js',
                function () {
                    $.getScript(layoutsRoot + 'SP.UI.Controls.js', renderSPChrome);
                    $.getScript(layoutsRoot + 'SP.RequestExecutor.js', function () {
                        var context = new SP.ClientContext(ParametersApp.SPAppWebUrl);
                        var factory = new SP.ProxyWebRequestExecutorFactory(ParametersApp.SPAppWebUrl);
                        if ($('#divField' + item).length > 0 && orden == undefined) {
                            context.set_webRequestExecutorFactory(factory);
                            Picker = new CAMControl.PeoplePicker(context, $('#span' + item), $('#input' + item), $('#div' + item + 'Search'), $('#hdn' + item));
                            Picker.InstanceName = "Picker";
                            Picker.Language = ParametersApp.SPLanguage;
                            Picker.MaxUsers = maxUsers;
                            Picker.Initialize();
                        } else if ($('#divField' + item).length > 0 && orden == 1) {
                            context.set_webRequestExecutorFactory(factory);
                            Picker1 = new CAMControl.PeoplePicker(context, $('#span' + item), $('#input' + item), $('#div' + item + 'Search'), $('#hdn' + item));
                            Picker1.InstanceName = "Picker1";
                            Picker1.Language = ParametersApp.SPLanguage;
                            Picker1.MaxUsers = maxUsers;
                            Picker1.Initialize();
                        } else if ($('#divField' + item).length > 0 && orden == 2) {
                            context.set_webRequestExecutorFactory(factory);
                            Picker2 = new CAMControl.PeoplePicker(context, $('#span' + item), $('#input' + item), $('#div' + item + 'Search'), $('#hdn' + item));
                            Picker2.InstanceName = "Picker2";
                            Picker2.Language = ParametersApp.SPLanguage;
                            Picker2.MaxUsers = maxUsers;
                            Picker2.Initialize();
                        } else if ($('#divField' + item).length > 0 && orden == 3) {
                            context.set_webRequestExecutorFactory(factory);
                            Picker3 = new CAMControl.PeoplePicker(context, $('#span' + item), $('#input' + item), $('#div' + item + 'Search'), $('#hdn' + item));
                            Picker3.InstanceName = "Picker3";
                            Picker3.Language = ParametersApp.SPLanguage;
                            Picker3.MaxUsers = maxUsers;
                            Picker3.Initialize();
                        } else if ($('#divField' + item).length > 0 && orden == 4) {
                            context.set_webRequestExecutorFactory(factory);
                            Picker4 = new CAMControl.PeoplePicker(context, $('#span' + item), $('#input' + item), $('#div' + item + 'Search'), $('#hdn' + item));
                            Picker4.InstanceName = "Picker4";
                            Picker4.Language = ParametersApp.SPLanguage;
                            Picker4.MaxUsers = maxUsers;
                            Picker4.Initialize();
                        }
                    });

                });
        });
}

function createPeoplePicker(context, divContener, control, divControlBusqueda, controlHdn, spLanguage, instancia, MaxUsers, ServerDataMethod, GrupoName) {
    var newPicker;
    newPicker = new CAMControl.PeoplePicker(context, divContener, control, divControlBusqueda, controlHdn, ServerDataMethod);
    newPicker.InstanceName = instancia;
    newPicker.Language = spLanguage;
    newPicker.MaxEntriesShown = 3;
    newPicker.AllowDuplicates = false;
    newPicker.ShowLoginName = true;
    newPicker.ShowTitle = true;
    newPicker.PrincipalType = 1;
    newPicker.MaxUsers = MaxUsers;
    newPicker.MinimalCharactersBeforeSearching = 4;
    newPicker.Initialize();
    return newPicker;
}function RedirectParent(url) {
    window.parent.location.replace(url);
}

function ValidarForm(form) {
    var MsjObligatorio = "<small class='form-text text-muted field-validation-error'>Campo {0} obligatorio</small>";

    var validacion = true;
    $(".form-text").remove();
    $("#" + form + " .form-control").removeClass("hasError");
    $("#" + form + " .custom-file").removeClass("hasError");
    $("#" + form + " div").removeClass("hasError");
    $("#" + form + " input").removeClass("hasErrorCheckBox");
    $("#" + form + " .required").each(function () {
        if (this.type == "checkbox") {
            if (!$(this).prop('checked')) {
                $(this).addClass("hasErrorCheckBox");
                validacion = false;
            }
        } else if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "[]") {
                if ($(this).hasClass("reference")) {
                    var reference = this.className.split("ref_")[1];
                    var objreference = $("#" + reference);
                    if (objreference[0].type == "checkbox") {
                        if ($(this).hasClass("controlSiNo")) {
                            if (objreference.prop('checked')) {
                                $(this.parentElement).addClass("hasError");
                                validacion = false;
                            }
                        } else {
                            if (!objreference.prop('checked')) {
                                $(this).addClass("hasError");
                                validacion = false;
                            }
                        }
                    } else {
                        objreference.addClass("hasError");
                        validacion = false;
                    }
                } else {
                    $(this).parent().addClass("hasError");
                    var Label = $("label[for='" + $(this).attr("id") + "']").text();

                    $(this).parent().parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                    validacion = false;
                }
            }
        } else if (this.localName == "p") {
            if ($(this).html() == "") {
                $(this).addClass("hasError");
                validacion = false;
            }
        } else if (this.localName == "table") {
            if ($("#table_documento_apoyo tbody").html().indexOf("No se encuentra resultados") != -1) {
                $(this).parent().addClass("hasError");
                validacion = false;
            }
        }
    });
    Communica.Part.adjustSize();
    return validacion;
}

function ValidarForm(form) {
    var MsjObligatorio = "<small class='form-text text-muted field-validation-error'>Campo {0} obligatorio</small>";

    var validacion = true;
    $(".form-text").remove();
    $("#" + form + " .form-control").removeClass("hasError");
    $("#" + form + " .custom-file").removeClass("hasError");
    $("#" + form + " div").removeClass("hasError");
    $("#" + form + " ul").removeClass("hasError");
    $("#" + form + " li").removeClass("hasError");
    $("#" + form + " input").removeClass("hasErrorCheckBox");
    $("#" + form + " .required").each(function () {
        if (this.type == "checkbox") {
            if (!$(this).prop('checked')) {
                $(this).addClass("hasErrorCheckBox");
                validacion = false;
            }
        } else if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "[]") {
                if ($(this).hasClass("reference")) {
                    var reference = this.className.split("ref_")[1];
                    var objreference = $("#" + reference);
                    if (objreference[0].type == "checkbox") {
                        if ($(this).hasClass("controlSiNo")) {
                            if (objreference.prop('checked')) {
                                $(this.parentElement).addClass("hasError");
                                validacion = false;
                            }
                        } else {
                            if (!objreference.prop('checked')) {
                                $(this).addClass("hasError");
                                validacion = false;
                            }
                        }
                    } else {
                        objreference.addClass("hasError");
                        validacion = false;
                    }
                } else {
                    $(this).parent().addClass("hasError");
                    var Label = $("label[for='" + $(this).attr("id") + "']").text();

                    $(this).parent().parent().append(MsjObligatorio.replace("{0}", Label.replace(":", "")));
                    validacion = false;
                }
            }
        } else if (this.localName == "p") {
            if ($(this).html() == "") {
                $(this).addClass("hasError");
                validacion = false;
            }
        } else if (this.localName == "table") {
            if ($("#table_documento_apoyo tbody").html().indexOf("No se encuentra resultados") != -1) {
                $(this).parent().addClass("hasError");
                validacion = false;
            }
        }
    });
    Communica.Part.adjustSize();
    return validacion;
}

function refreshoptiontable() {
    $(".dropdown-toggle").on("click", function () {
        if ($("ul.dropdown-menu").is(":visible")) {
            $("ul.dropdown-menu").hide();
        } else {
            $("ul.dropdown-menu").show();
        }
    });
}
function EnviarFormulario(formData, urlaction, fn_succes) {
    $.ajax({
        url: urlaction,
        type: 'POST',
        dataType: "Json",
        contentType: false,
        processData: false,
        data: formData,
        async: true,
        success: function (jResultado) {
            if (jResultado.Error == true) {
                App.MsjAjaxWarning(jResultado.Mensaje);
            }
            else {
                fn_succes(jResultado);
            }
        },
    });
}

function DownloadFileUrl(fileName, url) {
    fetch(url).then(function (t) {
        return t.blob().then((b) => {
            var a = document.createElement("a");
            a.href = URL.createObjectURL(b);
            a.setAttribute("download", filename);
            a.click();
        }
        );
    });
}

function GetFileNameUrl(Url) {
    var salida = "";
    if (Url) {
        salida = Url.split('/').pop();
    }
    return salida;
}