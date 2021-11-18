var Utils = {
    initial: function () {
        this.setCalendarLenguage();
        moment.locale('es');
    },
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
            yearSuffix: ''
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
        $("#cargando").show();
    },
    LoaderHide: function () {
        $("#cargando").hide();
    },
    MsjAjaxError: function () {
        swal('Error', "Ocurrió un error, por favor comuníquese con el administrador del sistema", 'error').catch(swal.noop);
    },
    MsjAjaxWarning: function () {
        swal('Aviso', "No se puedo realizar la acción, por favor comuníquese con el administrador del sistema", 'warning').catch(swal.noop);
    },
    MsjAjaxSuccess: function () {
        swal({ type: 'success', title: 'Éxito', text: "Se registro correctamente.", showConfirmButton: false, timer: 2000 })
    },
    validarcampos: function (form) {
        var validacion = true;
        Utils.LimpiarValidacionFrm(form)
        $("#" + form + " .required:enabled").each(function () {
            if (this.type == "checkbox") {
                if (!$(this).prop('checked')) {
                    $(this).addClass("hasErrorCheckBox");
                    validacion = false;
                }
            } else if (this.localName == "input" || this.localName == "select" || this.localName == "textarea") {
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "[]") {
                    if ($(this).hasClass("reference")) {
                        var reference = this.className.split("ref_")[1];
                        $("#" + reference).addClass("hasError");
                    } else {
                        $(this).addClass("hasError");
                    }
                    validacion = false;
                }
            }
        });
        return validacion;
    },
    LimpiarValidacionFrm: function (form) {
        $("#" + form + " .form-control").removeClass("hasError");
        $("#" + form + " div").removeClass("hasError");
        $("#" + form + " input").removeClass("hasError");
    },
    swalExito: function (_message) {
        swal({
            type: 'success', title: 'Éxito', text: _message,
            allowOutsideClick: false,
            allowEscapeKey: false
        });
    },
    closeForm: function () {
        Utils.LoaderShow();
        window.parent.location.replace(SPOAppParameters.SPHost);
    },
    swalWarning: function (_message) {
        Utils.LoaderHide();
        swal('Aviso', _message, 'warning').catch(swal.noop);
    },
    swalError: function (_message) {
        Utils.LoaderHide();
        swal('Error', _message, 'error').catch(swal.noop);
    },
    swalConfirm: function (_message, fn_succes, option_succes) {
        Utils.LoaderHide();
        swal({
            title: "",
            text: _message,
            type: "question",
            showCancelButton: true,
            confirmButtonText: "Si",
            cancelButtonText: "No",
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            buttonsStyling: false,
            reverseButtons: true
            //target: '#botonera-registrar'
        }).then(function (result) {
            if (result) {
                if (option_succes != undefined)
                    fn_succes(option_succes);
                else
                    fn_succes();
            }
        });
    },
    swalConfirmWithComents: function (_message, fn_succes, placeholderText, option_succes) {
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
    },
    swalExitoRedirect: function (_message, url) {
        Utils.LoaderHide();
        swal({
            type: 'success', title: 'Éxito', text: _message,
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then(function (result) {
            //if (result) {
            Utils.LoaderShow();
            window.parent.location.replace(url);
            //}
        });
    },
    getQueryStringParameter: function (urlParameterKey) {
        var params = document.URL.split('?')[1].split('&');
        var strParams = '';
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split('=');
            if (singleParam[0] == urlParameterKey)
                return decodeURIComponent(singleParam[1]);
        }
    },
    bufferToBinary: function (buffer) {
        var bytes = new Uint8Array(buffer);
        var binary = '';
        for (var b = 0; b < bytes.length; b++) {
            binary += String.fromCharCode(bytes[b]);
        }
        return binary;
    },
    bindTable: function (tableid, jsonData) {
        var $table = $(tableid);
        $.each(jsonData, function (i, row) {
            row.index = i;
            row.OpDelete = "";
        });
        $table.bootstrapTable("destroy");
        $table.bootstrapTable({
            data: jsonData
        });
        $(".numerico").numeric({ decimalPlaces: 0, negative: false });
    }
};


$(document).ready(function () {
    Utils.initial();
});

function leftPad(value, length) {
    return ('0'.repeat(length) + value).slice(-length);
}