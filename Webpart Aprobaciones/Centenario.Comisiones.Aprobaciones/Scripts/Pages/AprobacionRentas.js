var Rentas = {};
var AprobacionRentas = {
    initial: function () {
        Rentas.ListaHistorial = 'Historial Aprobaciones';
        Rentas.ListaEjecutivosMinka = 'Ejecutivos Minka';
        Rentas.ListaEjecutivosRI = 'Ejecutivos Renta Inmobiliaria';
        Rentas.ListaRoles = 'Roles';
        Rentas.Actualizado = false;
        Rentas.Roles = [];
        Rentas.Ejecutivos = [];
        AprobacionRentas.initialFields();
    },
    initialFields: function () {
        $('#table-comisiones').bootstrapTable();
        AprobacionRentas.cargarAnios();
        $('#btnBuscar').click(AprobacionRentas.buscarComisiones);
        $('#btnAprobar').click(AprobacionRentas.aprobarComisiones);
        $('#btnRechazar').click(AprobacionRentas.rechazarComisiones);
    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = year; i >= 2020; i--) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
        $('#ddlAnio').val(year);
    },
    CargarComisiones: function (data) {
        Rentas.DatosHistorial = data;
        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        if (Rentas.DatosHistorial <= 0) {
            table.bootstrapTable();
        }
        else {
            table.bootstrapTable({
                data: Rentas.DatosHistorial
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
        funcSPO.getListItemsFilter(Rentas.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", AprobacionRentas.CargarComisiones, null, true, '/rentas');
    },
    CargarEjecutivos: function (data) {
        $(data).each(function (i, val) {
            var Exists = Rentas.Ejecutivos.filter(x => x.EjecutivoId == val.EjecutivoId);
            if (Exists.length <= 0 || Exists == undefined) {
                Rentas.Ejecutivos.push(val)
                var ExisteRol = Rentas.Roles.find(function (n) {
                    return n.Codigo == val.Rol.Codigo;
                });
                if (ExisteRol == undefined)
                    Rentas.Roles.push(val.Rol.Codigo);
            }
        });
    },
    cargarComisionesAprobar: function () {
        AprobacionRentas.buscarComisiones();
    },
    aprobarComisiones: function () {
        var aprobar = $("#table-comisiones").bootstrapTable('getSelections');
        if (aprobar.length == 0)
            Utils.swalWarning('Debe seleccionar al menos un registro para aprobar.');
        else {
            Rentas.Seleccionados = aprobar.length;
            Rentas.Procesados = 0;
            Utils.LoaderShow();
            $(aprobar).each(function (i, val) {
                var item = {
                    "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
                    "Estado": "Aprobado",
                    "AprobadorId": SPOAppParameters.CurrentUser.Id,
                    "FechaAprobacion": moment().format('YYYY-MM-DD')
                };
                funcSPO.updateListItem(Rentas.ListaHistorial, item, val.ID, AprobacionRentas.succesAprobar, AprobacionRentas.errorSPO, '/rentas');
            });
        }
    },
    rechazarComisiones: function () {
        var aprobar = $("#table-comisiones").bootstrapTable('getSelections');
        if (aprobar.length == 0)
            Utils.swalWarning('Debe seleccionar al menos un registro para rechazar.');
        else {
            Rentas.Seleccionados = aprobar.length;
            Rentas.Procesados = 0;
            Utils.LoaderShow();
            $(aprobar).each(function (i, val) {
                var item = {
                    "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
                    "Estado": "Rechazado",
                    "AprobadorId": SPOAppParameters.CurrentUser.Id
                };
                funcSPO.updateListItem(Rentas.ListaHistorial, item, val.ID, AprobacionRentas.succesRechazar, AprobacionRentas.errorSPO, '/rentas');
            });
        }
    },
    succesAprobar: function () {
        console.log(Rentas.Procesados);
        Rentas.Procesados++;
        if (Rentas.Procesados == Rentas.Seleccionados) {
            AprobacionRentas.buscarComisiones();
            Utils.swalExito("Se aprobaron los registros seleccionados");
        }
    },
    succesRechazar: function () {
        Rentas.Procesados++;
        if (Rentas.Procesados == Rentas.Seleccionados) {
            AprobacionRentas.buscarComisiones();
            Utils.swalExito("Se rechazaron los registros seleccionados");
        }
    },
    buscarComisiones: function () {
        Utils.LoaderShow();
        var filter = "Estado eq 'Pendiente' and RolAprobador eq '" + Rentas.Roles[0] + "' and Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(Rentas.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", AprobacionRentas.CargarComisiones, null, true, '/rentas');
    }
}

$(document).ready(function () {
    AprobacionRentas.initial();
    Communica.Part.adjustSize();
    funcSPO.getCurrentUser();
    funcSPO.getListItemsFilter(Rentas.ListaEjecutivosMinka, "EjecutivoId eq " + SPOAppParameters.CurrentUser.Id + "&$select=Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Rol/ID,Rol/Title,Rol/Codigo&$expand=Ejecutivo,Rol", AprobacionRentas.CargarEjecutivos, null, true, '/rentas');
    funcSPO.getListItemsFilter(Rentas.ListaEjecutivosRI, "EjecutivoId eq " + SPOAppParameters.CurrentUser.Id + "&$select=Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Rol/ID,Rol/Title,Rol/Codigo&$expand=Ejecutivo,Rol", AprobacionRentas.CargarEjecutivos, null, true, '/rentas');
    AprobacionRentas.buscarComisiones();
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