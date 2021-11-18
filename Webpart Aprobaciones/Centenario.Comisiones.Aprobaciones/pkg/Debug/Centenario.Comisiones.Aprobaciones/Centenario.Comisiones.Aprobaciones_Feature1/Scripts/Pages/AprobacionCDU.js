var CDU = {};
var SolicitarAprobacionCDU = {
    initial: function () {
        CDU.ListaHistorial = 'Historial Aprobaciones';
        CDU.ListaEjecutivosIndustrial = 'Ejecutivos Industrial';
        CDU.ListaEjecutivosUrbanizacion = 'Ejecutivos Urbanizaciones';
        CDU.ListaRoles = 'Roles';
        CDU.Actualizado = false;
        CDU.Roles = [];
        CDU.Ejecutivos = [];
        SolicitarAprobacionCDU.initialFields();
    },
    initialFields: function () {
        $('#table-comisiones').bootstrapTable();
        SolicitarAprobacionCDU.cargarAnios();
        $('#btnBuscar').click(SolicitarAprobacionCDU.buscarComisiones);
        $('#btnAprobar').click(SolicitarAprobacionCDU.aprobarComisiones);
        //SolicitarAprobacionCDU.buscarComisiones();
    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = 2020; i < year + 1; i++) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
        $('#ddlAnio').val(year);
    },
    CargarComisiones: function (data) {
        CDU.DatosHistorial = data;
        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        if (CDU.DatosHistorial <= 0) {
            table.bootstrapTable();
        }
        else {
            table.bootstrapTable({
                data: CDU.DatosHistorial
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
        funcSPO.getListItemsFilter(CDU.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCDU.CargarComisiones, null, true, '/du');
    },
    CargarEjecutivos: function (data) {
        //CDU.DatosHistorial = data;
        $(data).each(function (i, val) {
            CDU.Ejecutivos.push(val)
            var ExisteRol = CDU.Roles.find(function (n) {
                return n.Codigo == val.Rol.Codigo;
            });
            if (ExisteRol == undefined)
                CDU.Roles.push(val.Rol.Codigo);
        });
    },
    cargarComisionesAprobar: function () {
        SolicitarAprobacionCDU.buscarComisiones()
        //$(CDU.Roles).each(function (i, val) {
        //    switch (val) {
        //        case '002':
        //            break;
        //        default:
        //    }
        //});
    },
    aprobarComisiones: function () {
        var aprobar = $("#table-comisiones").bootstrapTable('getSelections');
        if (aprobar.length == 0)
            Utils.swalWarning('Debe seleccionar al menos un registro para aprobar.');
        else {
            $(aprobar).each(function (i, val) {
                var item = {
                    "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
                    "Estado": "Aprobado"
                };
                funcSPO.updateListItem(CDU.ListaHistorial, item, val.ID, SolicitarAprobacionCDU.succesAprobar, SolicitarAprobacionCDU.errorSPO, '/du');
            });
            //var row = select[0];
            //Solicitud.CodigoPerfil = row.OBJID;
            //Solicitud.Opcion = opcion;
            //if (Solicitud.Opcion == 'MODIFICAR') {
            //    Utils.LoaderShow();
            //    funcSPO.getListItemsFilter(Solicitud.ListaSolicitudes, "CodigoFuncion eq '" + row.OBJID + "' and Estado/Codigo ne 'E005' and Estado/Codigo ne 'E006'and Estado/Codigo ne 'E007'&$select=ID,Title,Estado/Codigo&$expand=Estado", fncBusqueda.onSuccessSolicitudesExistente);
            //}
            //else
            //    fncBusqueda.redirectPerfil();
        }
    },
    succesAprobar: function () {
         
    },
    //buscarComisiones: function () {
    //    Utils.LoaderShow();
    //    var filter = "EjecutivoId eq " + CDU.IdEjec + " and Anio eq " + $('#ddlAnio').val();
    //    if ($('#ddlMes').val() != "0")
    //        filter = filter + " and Mes eq " + $('#ddlMes').val();
    //    funcSPO.getListItemsFilter(CDU.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCDU.CargarComisiones, null, true, '/du');
    //}
}

$(document).ready(function () {
    SolicitarAprobacionCDU.initial();
    Communica.Part.adjustSize();
    funcSPO.getCurrentUser();
    funcSPO.getListItemsFilter(CDU.ListaEjecutivosIndustrial, "EjecutivoId eq " + SPOAppParameters.CurrentUser.Id + "&$select=Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Supervisor/ID,Supervisor/Title,Rol/ID,Rol/Title,Rol/Codigo&$expand=Ejecutivo,Rol,Supervisor", SolicitarAprobacionCDU.CargarEjecutivos, null, true, '/du');
    funcSPO.getListItemsFilter(CDU.ListaEjecutivosUrbanizacion, "EjecutivoId eq " + SPOAppParameters.CurrentUser.Id + "&$select=Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Supervisor/ID,Supervisor/Title,Rol/ID,Rol/Title,Rol/Codigo&$expand=Ejecutivo,Rol,Supervisor", SolicitarAprobacionCDU.CargarEjecutivos, null, true, '/du');
    SolicitarAprobacionCDU.buscarComisiones();
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