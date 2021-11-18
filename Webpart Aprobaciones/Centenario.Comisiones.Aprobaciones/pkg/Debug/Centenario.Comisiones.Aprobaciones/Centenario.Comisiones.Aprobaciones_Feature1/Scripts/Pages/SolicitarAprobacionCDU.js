var CDU = {};
var SolicitarAprobacionCDU = {
    initial: function () {
        CDU.ListaHistorial = 'Historial Aprobaciones';
        CDU.ListaRoles = 'Roles';
        CDU.ListaEjecutivosIndustrial = 'Ejecutivos Industrial';
        CDU.ListaEjecutivosUrbanizacion = 'Ejecutivos Urbanizaciones';
        CDU.Actualizado = false;
        SolicitarAprobacionCDU.initialFields();
    },
    initialFields: function () {

        CDU.IdRol = decodeURIComponent(Utils.getQueryStringParameter('IDROL'));
        CDU.IdEjec = decodeURIComponent(Utils.getQueryStringParameter('IDEJEC'));
        CDU.Anio = decodeURIComponent(Utils.getQueryStringParameter('ANIO'));
        CDU.Mes = decodeURIComponent(Utils.getQueryStringParameter('MES'));
        CDU.TotCom = decodeURIComponent(Utils.getQueryStringParameter('TOTCOM'));
        if (CDU.TotCom != undefined)
            CDU.TotCom = CDU.TotCom.replace(',', '.')
        $('#table-comisiones').bootstrapTable();
        SolicitarAprobacionCDU.cargarAnios();

        $('#ddlMes').val(CDU.Mes);
        $('#ddlAnio').val(CDU.Anio);
        $('#btnBuscar').click(SolicitarAprobacionCDU.buscarComisiones);
        SolicitarAprobacionCDU.buscarComisiones();

    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = 2020; i < year + 1; i++) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
    },
    CargarComisiones: function (data) {
        CDU.DatosHistorial = data;

        var item = {
            "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
            "RolId": CDU.IdRol,
            "EjecutivoId": CDU.IdEjec,
            "Anio": CDU.Anio,
            "Mes": CDU.Mes,
            "Estado": "Pendiente",
            "TotalComision": parseFloat(CDU.TotCom).toFixed(2),
            "Unidad": "",
        };

        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        if (CDU.DatosHistorial <= 0) {
            if ($('#ddlAnio').val() == CDU.Anio && $('#ddlMes').val() == CDU.Mes)
                funcSPO.addListItem(CDU.ListaHistorial, item, SolicitarAprobacionCDU.buscarComisiones, SolicitarAprobacionCDU.errorSPO, '/du');
            table.bootstrapTable();
        }
        else {
            var _Item = CDU.DatosHistorial[0];
            if (_Item.Estado != "Aprobado" && !CDU.Actualizado && CDU.Mes != "0" && $('#ddlAnio').val() == CDU.Anio && $('#ddlMes').val() == CDU.Mes) {

                CDU.Actualizado = true;
                funcSPO.updateListItem(CDU.ListaHistorial, item, _Item.ID, SolicitarAprobacionCDU.buscarComisiones, SolicitarAprobacionCDU.errorSPO, '/du');
            }
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
        var filter = "EjecutivoId eq " + CDU.IdEjec + " and Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(CDU.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCDU.CargarComisiones, null, true, '/du');
    }
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
    FormatoInputNumerico: function (value, row, index) {
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    },
    FormatDecimal: function (value, row, index) {
        return 'S/ ' + value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    },
}