(function () {
    this.app = {
        LoaderShow: function () {
            $("#loader").show();
        },
        LoaderHide: function () {
            $("#loader").hide();
        },
        MsjAjaxSuccces: function (msj) {
            swal({
                type: 'success',
                title: 'Exito',
                text: msj + ', se realizó correctamente.',
                showConfirmButton: true,
                timer: 1500
            }).catch(swal.noop);
        },
        MsjAjaxError: function () {
            swal('Error', "Ocurrió un error, por favor comuníquese con el administrador del sistema.", 'error').catch(swal.noop);
        },
        MsjAjaxWarning: function (msj) {
            swal('Aviso', msj, 'warning').catch(swal.noop);
        },
    };
    this.msj = {
        Registro: "Se registró",
        Actualizo: "Se Actualizó",
        Elimino: "Se eliminó",
        ConfirmaGuardar: "¿Está seguro de guardar el registro?",
        EliminarRegistro: "¿Está seguro de eliminar el registro {0}?",
    }
}).call(this);

$.fn.modal.prototype.constructor.Constructor.DEFAULTS.backdrop = 'static';

$.ajaxSetup({
    beforeSend: function () {
        app.LoaderShow();
    },
    complete: function () {
        app.LoaderHide();
    },
    error: function (a, b, e) {
        app.LoaderHide();
        app.MsjAjaxError();
    }
});

var MensajeValidacion = function (campo) {
    return "Campo " + campo + " obligatorio";
}

var MensajeIncorrecto = function (campo) {
    return "Campo " + campo + " incorrecto";
}

function ClearValidarCampos() {
    $(".help-block").remove();
    $(".form-group").removeClass("has-error");
    $(".form-group div").removeClass("has-error");
    $(".panel-body .row div").removeClass("has-error");
}

var txtNumeroDocumentoDefault = function (txt) {
    txt.numeric({ decimal: false, negative: false });
    txt.attr("maxlength", 20);
}

var ControlTipoDocumento = function (cbx, txt) {
    if (cbx.val() == 0 || cbx.val() == null || cbx.val() == "") {
        txtNumeroDocumentoDefault(txt);
    } else {
        cbxTipoDocumento(cbx, txt);
    }
    cbx.change(function () {
        txt.val("")
        if (cbx.val() == 0 || cbx.val() == null || cbx.val() == "") {
            txtNumeroDocumentoDefault(txt);
        } else {
            cbxTipoDocumento(cbx, txt);
        }
    });
}

var cbxTipoDocumento = function (cbx, txt) {
    var atributo = $(cbx.selector + ">option:selected").attr("data-atributo");
    var longitud = $(cbx.selector + ">option:selected").attr("data-longitud");
    var exacto = $(cbx.selector + ">option:selected").attr("data-exacto");
    if (atributo == "N") {
        txt.numeric({ decimal: false, negative: false });
    } else {
        txt.removeNumeric();
    }
    if (exacto == 1) {
        txt.attr("minlength", longitud);
        txt.attr("maxlength", longitud);
    } else {
        txt.attr("maxlength", longitud);
    }
}

function swalConfirm(_message, fn_succes, option_succes) {
    swal({
        title: "Confirmación",
        text: _message,
        type: "question",
        showCancelButton: true,
        confirmButtonText: "Si",
        confirmButtonColor: '#3085d6',
        cancelButtonText: "No"
    }).then(function (result) {
        if (result.value) {
            if (option_succes != undefined)
                fn_succes(option_succes);
            else
                fn_succes();
        }
    }).catch(swal.noop);
}

function swalOK(_message, fn_succes, option_succes) {
    swal({
        title: "Mensaje",
        text: _message,
        type: "success",
        showCancelButton: false,
        confirmButtonText: "OK",
        cancelButtonText: "No",
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false,
        reverseButtons: true
        //target: '#botonera-registrar'
    }).then(function (result) {
        if (result) {
            if (fn_succes != undefined) {
                if (option_succes != undefined)
                    fn_succes(option_succes);
                else
                    fn_succes();
            }
        }
    }).catch(swal.noop);
}

function swalConfirmWithComents(_message, fn_succes, placeholderText, option_succes) {
    swal({
        title: "Confirmación",
        text: _message,
        type: "question",
        input: 'textarea',
        inputPlaceholder: placeholderText,
        inputClass: 'form-control',
        showCancelButton: true,
        confirmButtonText: "Si",
        cancelButtonText: "No",
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false,
        reverseButtons: true,
        inputValidator: function (value) {
            return new Promise(function (resolve, reject) {
                if (value === '') {
                    reject('Este dato es obligatorio.')
                } else {
                    resolve()
                }
            })
        },
        allowOutsideClick: false
        //target: '#botonera-registrar'
    }).then(function (result) {
        if (result != "") {
            if (option_succes != undefined)
                fn_succes(result, option_succes);
            else
                fn_succes(result);
        }
    });
}

function swalExitoRedirect(_message, url) {
    swal({
        type: 'success', title: 'Éxito', text: _message,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function (result) {
        //if (result) {
        app.LoaderShow();
        RedirectParent(url);
        //}
    });
}

var LocationHref = function (url) {
    window.location.href = url;
}

var LocationReplace = function (url) {
    window.parent.location.replace(url);
}

function AbrirPopupDeVista(idModal, urlvista, param) {
    $.ajax({
        type: 'POST',
        url: urlvista,
        data: param,
        async: true,
        beforeSend: function (data) {
            app.LoaderShow();
        },
        success: function (data) {
            $("#" + idModal).html(data);
            $("#" + idModal).modal("show");
        },
        error: function (a, b, e) {
            app.LoaderHide();
            swal('Error', "Ocurrió un error, por favor comuníquese con el administrador del sistema", 'error').catch(swal.noop);
        },
        complete: function (obj, status) {
            app.LoaderHide();
        }
    });
}

//==============Formats BoostrapTable =======================
function opFormNroItemTabla(value, row, index) {
    return index + 1;
}

function opFormSiNoTable(value, row, index) {
    return value == 0 ? 'No' : 'Sí';
}

function dateFormatTable(value, row, index) {
    moment.locale('es');
    return moment(value).format('DD/MM/YYYY');
}

function decimalFormatTable(value, row, index) {
    return Number(value).toFixed(2);
}

function PercentFormatTable(value, row, index) {
    return value.toLocaleString("en-US", { style: "percent" })
}

$(document).ready(function () {
    $(".bootstrap-table>div>div.search>input").addClass("text-uppercase");
    $('.modal').on('show.bs.modal', function () {
        _initial_body_overflow = $('body').css('overflow');
        $(this).css('overflow-y', 'auto');
        $('body').css('padding-right', '0');
    }).on('hide.bs.modal', function () {
        $(this).css('overflow-y', 'hidden');
        $('body').css('overflow', _initial_body_overflow);
        $('body').css('padding-right', '0');
    });
});

var ControlRangoFecha = function (DtIni, txtIni, hddIni, DtFin, txtFin, hddFin) {
    DtIni.datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        orientation: "auto",
        todayHighlight: true
    }).on('changeDate', function (e) {
        DtFin.datepicker("setStartDate", (e.format(0, "dd/mm/yyyy")));
    });
    DtFin.datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        orientation: "auto",
        todayHighlight: true
    }).on('changeDate', function (e) {
        DtIni.datepicker("setEndDate", (e.format(0, "dd/mm/yyyy")));
    });
    if ($(txtIni).val() != "") {
        DtFin.datepicker("setStartDate", $(txtIni).val());
    }
    if ($(txtFin).val() != "") {
        DtIni.datepicker("setEndDate", $(txtFin).val());
    }
    txtIni.change(function () {
        if ($(this).val() == "") {
            DtFin.datepicker("setStartDate", "");
        }
        if (txtFin.val() == "") {
            DtIni.datepicker("setEndDate", "");
        }
        ChangeDateFormat(txtFin, hddIni);
    })
    txtFin.change(function () {
        if ($(this).val() == "") {
            DtIni.datepicker("setEndDate", "");
        }
        if (txtIni.val() == "") {
            DtFin.datepicker("setStartDate", "");
        }
        ChangeDateFormat(txtIni, hddFin);
    })
}

function ChangeDateFormat(txtDate, hddDate) {
    let val = txtDate.val();
    if (val != "") {
        let datePart = val.split("/")
        val = (datePart[1] + "/" + datePart[0] + "/" + datePart[2]);
    }
    hddDate.val(val);
}

var RangoFechaDefault = function (DtIni, txtIni, hddIni, DtFin, txtFin, hddFin) {
    DtIni.datepicker("setEndDate", "");
    txtIni.val("");
    hddIni.val("");
    DtFin.datepicker("setStartDate", "");
    txtFin.val("");
    hddFin.val("");
}

function TextOpcionFormatter(value, row, index) {
    if (value == "1") {
        return 'Si';
    }
    else if (value == "0") {
        return 'No';
    }
}

function FormatActivo(value, row, index) {
    if (value == "1") {
        return ['<span style="font-size: 15px; color: #5cb85c;" class="fa fa-circle">',].join('');
    }
    else {
        return ['<span style="font-size: 15px; color: #f29a1c;" class="fa fa-circle">',].join('');
    }
}

function checkboxFormatter(value, row, index) {
    if (value == "1") {
        return ['<input type="checkbox" checked disabled />',].join('');
    }
    else {
        return ['<input type="checkbox" disabled>',].join('');
    }
}

function archivoUrlFormatter(value, row, index) {
    if (value != "") {
        return ['<a href="' + value + '" target ="_blank" class="btn btn-default btn-xs" title="Descargar"><span class="fa fa-download" aria-hidden="true"></span></a>'].join('');
    }
}

function opFormNro(value, row, index) {
    var pageSize = $('span.page-size').html()
    var pagNumber = $('.pagination > li.active > a').html() - 1;
    return index + pageSize * pagNumber + 1;
}

function FormatOpFecha(value, row, index) {
    return FormatoFecha(value);
}

var FormatoFecha = function (value) {
    if (value == null || value == "") return ""
    n = moment(value).format('DD/MM/YYYY');
    return n;
}

function PoblarCombo(url, campo, parametros, codigo, valor, selectedValue) {
    $("#" + campo).empty();
    if (selectedValue != null && selectedValue != "") {
        $("#" + campo).append("<option value='-1'>-- Seleccionar --</option>");
    } else {
        $("#" + campo).append("<option value='-1' selected>-- Seleccionar --</option>");
    }
    $.getJSON(url + parametros, function (data) {
        $.each(data, function (index, dato) {
            if (this.CITEM == selectedValue) {
                $("#" + campo).append("<option value=\"" + this.CITEM + "\" selected>" + this.DITEM + "</option>");
            } else {
                $("#" + campo).append("<option value=\"" + this.CITEM + "\">" + this.DITEM + "</option>");
            }
        });
    });
    if (selectedValue != null && selectedValue != "" && selectedValue != "0" && selectedValue != "-1" && selectedValue != "null") {
        $("#" + campo).val("\"" + selectedValue + "\"").trigger('change')
    } else {
        $("#" + campo).val('-1').trigger('change')
    }
    $("#" + campo).css("width", "100%").select2({ language: { noResults: function (term) { return "No se encontraron registros" } } });
}

function decodificarEntidadesHTMLNumericas(texto) {
    return texto.replace(/&#(\d{1,8});/g, function (m, ascii) {
        return String.fromCharCode(ascii);
    });
}

var formatNumber = {
    separador: ",", // separador para los miles
    sepDecimal: '.', // separador para los decimales
    formatear: function (num) {
        num += '';
        var splitStr = num.split('.');
        var splitLeft = splitStr[0];
        var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
        var regx = /(\d+)(\d{3})/;
        while (regx.test(splitLeft)) {
            splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
        }
        return this.simbol + splitLeft + splitRight;
    },
    new: function (num, simbol) {
        this.simbol = simbol || '';
        return this.formatear(num);
    }
}

function ActivarOpcionMenu(control) {
    $("#px-demo-nav .px-nav-item").each(function (f) {
        $(this).removeClass('active');
    })
    $("#" + control).addClass("active");
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function FormatoFechaHora(value) {
    if (value == null) return ""
    n = moment(value).format('DD/MM/YYYY hh:mm a');
    return n;
}

function FormatoHora(value) {
    if (value == null) return ""
    n = moment(value).format('hh:mm a');
    return n;
}

function FormatoFechaNombre(value) {
    if (value == null) return ""
    n = moment(value).format('dd MMM yyyy');
    return n;
}

function formatDecimal_cuatro(value, row, index) {
    value += '';
    value = parseFloat(value.replace(/[^0-9\.]/g, ''));
    if (isNaN(value) || value === 0)
        return parseFloat(0).toFixed(4);
    value = '' + value.toFixed(4);
    var amount_parts = value.split('.'),
        regexp = /(\d+)(\d{3})/;
    while (regexp.test(amount_parts[0]))
        amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');
    return amount_parts.join('.');
}

function formatDecimal_tres(value, row, index) {
    value += '';
    value = parseFloat(value.replace(/[^0-9\.]/g, ''));
    if (isNaN(value) || value === 0)
        return parseFloat(0).toFixed(3);
    value = '' + value.toFixed(3);
    var amount_parts = value.split('.'),
        regexp = /(\d+)(\d{3})/;
    while (regexp.test(amount_parts[0]))
        amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');
    return amount_parts.join('.');
}

function formatDecimal_dos(value, row, index) {
    var num = value;
    if (num != "" && num != null) {
        num = parseFloat(num).toFixed(2);
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\,?)(\d{3})/g, '$1,');
        num = num.split('').reverse().join('').replace(/^[\,]/, '');
    } else {
        num = "";
    }
    return num;
}

function InputNumeric(e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;
    return /\d/.test(String.fromCharCode(keynum));
}