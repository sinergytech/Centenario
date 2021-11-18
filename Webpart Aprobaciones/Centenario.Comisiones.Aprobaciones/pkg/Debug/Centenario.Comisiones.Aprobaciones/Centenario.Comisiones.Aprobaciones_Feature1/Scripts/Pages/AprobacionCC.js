var CC = {};
var SolicitarAprobacionCC = {
    initial: function () {
        CC.ListaHistorial = 'Historial Aprobaciones';
        CC.ListaEjecutivos = 'Ejecutivo - Rol';
        CC.ListaRoles = 'Roles';
        CC.Actualizado = false;
        CC.Roles = [];
        CC.Ejecutivos = [];
        SolicitarAprobacionCC.initialFields();
    },
    initialFields: function () {
        $('#table-comisiones').bootstrapTable();
        SolicitarAprobacionCC.cargarAnios();
        $('#btnBuscar').click(SolicitarAprobacionCC.buscarComisiones);
        $('#btnAprobar').click(SolicitarAprobacionCC.aprobarComisiones);
        $('#btnRechazar').click(SolicitarAprobacionCC.rechazarComisiones);
    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = year; i >= 2020; i--) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
        $('#ddlAnio').val(year);
    },
    CargarComisiones: function (data) {
        CC.DatosHistorial = data;
        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        if (CC.DatosHistorial <= 0) {
            table.bootstrapTable();
        }
        else {
            table.bootstrapTable({
                data: CC.DatosHistorial
            });
        };
        Utils.LoaderHide();
        Communica.Part.adjustSize();
    },
    errorSPO: function (error) {
        Utils.swalError('Ocurrió un error: ' + error.message.value);
    },
    buscarComisiones: function () {
        Utils.LoaderShow();
        var filter = "Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(CC.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCC.CargarComisiones, null, true, '/cobranzas');
    },
    CargarEjecutivos: function (data) {
        $(data).each(function (i, val) {
            CC.Ejecutivos.push(val)
            var ExisteRol = CC.Roles.find(function (n) {
                return n.Codigo == val.Rol.Codigo;
            });
            if (ExisteRol == undefined)
                CC.Roles.push(val.Rol.Codigo);
        });
    },
    cargarComisionesAprobar: function () {
        SolicitarAprobacionCC.buscarComisiones();
    },
    aprobarComisiones: function () {
        var aprobar = $("#table-comisiones").bootstrapTable('getSelections');
        if (aprobar.length == 0)
            Utils.swalWarning('Debe seleccionar al menos un registro para aprobar.');
        else {
            CC.Seleccionados = aprobar.length;
            CC.Procesados = 0;
            Utils.LoaderShow();
            $(aprobar).each(function (i, val) {
                var item = {
                    "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
                    "Estado": "Aprobado",
                    "AprobadorId": SPOAppParameters.CurrentUser.Id,
                    "FechaAprobacion": moment().format('YYYY-MM-DD')
                };
                funcSPO.updateListItem(CC.ListaHistorial, item, val.ID, SolicitarAprobacionCC.succesAprobar, SolicitarAprobacionCC.errorSPO, '/cobranzas');
            });
        }
    },
    rechazarComisiones: function () {
        var aprobar = $("#table-comisiones").bootstrapTable('getSelections');
        if (aprobar.length == 0)
            Utils.swalWarning('Debe seleccionar al menos un registro para rechazar.');
        else {
            CC.Seleccionados = aprobar.length;
            CC.Procesados = 0;
            Utils.LoaderShow();
            $(aprobar).each(function (i, val) {
                var item = {
                    "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
                    "Estado": "Rechazado",
                    "AprobadorId": SPOAppParameters.CurrentUser.Id
                };
                funcSPO.updateListItem(CC.ListaHistorial, item, val.ID, SolicitarAprobacionCC.succesRechazar, SolicitarAprobacionCC.errorSPO, '/cobranzas');
            });
        }
    },
    succesAprobar: function () {
        console.log(CC.Procesados); 
        CC.Procesados++;
        if (CC.Procesados == CC.Seleccionados) {
            SolicitarAprobacionCC.buscarComisiones();
            Utils.swalExito("Se aprobaron los registros seleccionados");
        }
    },
    succesRechazar: function () {
        CC.Procesados++;
        if (CC.Procesados == CC.Seleccionados) {
            SolicitarAprobacionCC.buscarComisiones();
            Utils.swalExito("Se rechazaron los registros seleccionados");
        }
    },
    buscarComisiones: function () {
        Utils.LoaderShow();
        var filter = "Estado eq 'Pendiente' and RolAprobador eq '" + CC.Roles[0] + "' and Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(CC.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCC.CargarComisiones, null, true, '/cobranzas');
    }
}

$(document).ready(function () {
    SolicitarAprobacionCC.initial();
    Communica.Part.adjustSize();
    funcSPO.getCurrentUser();
    funcSPO.getListItemsFilter(CC.ListaEjecutivos, "EjecutivoId eq " + SPOAppParameters.CurrentUser.Id + "&$select=Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Supervisor/ID,Supervisor/Title,Rol/ID,Rol/Title,Rol/Codigo&$expand=Ejecutivo,Rol,Supervisor", SolicitarAprobacionCC.CargarEjecutivos, null, true, '/cobranzas');
    SolicitarAprobacionCC.buscarComisiones();
});

var tableFormats = {
    FormatMes: function (value, row, index) {
        var nombreMes = '';
        switch (value) {
            case 1:
                nombreMes = 'Enero';
                break;
            case 2:
                nombreMes = 'Febrero';
                break;
            case 3:
                nombreMes = 'Marzo';
                break;
            case 4:
                nombreMes = 'Abril';
                break;
            case 5:
                nombreMes = 'Mayo';
                break;
            case 6:
                nombreMes = 'Junio';
                break;
            case 7:
                nombreMes = 'Julio';
                break;
            case 8:
                nombreMes = 'Agosto';
                break;
            case 9:
                nombreMes = 'Setiembre';
                break;
            case 10:
                nombreMes = 'Octubre';
                break;
            case 11:
                nombreMes = 'Noviembre';
                break;
            case 12:
                nombreMes = 'Diciembre';
                break;
            default:
        }

        return nombreMes;
    },
    FormatDecimal: function (value, row, index) {
        return 'S/ ' + value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    },
}