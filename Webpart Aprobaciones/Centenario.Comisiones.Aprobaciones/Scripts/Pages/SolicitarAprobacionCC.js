var CC = {};
var SolicitarAprobacionCC = {
    initial: function () {
        CC.ListaHistorial = 'Historial Aprobaciones';
        CC.ListaRoles = 'Roles';
        CC.Actualizado = false;
        SolicitarAprobacionCC.initialFields();
    },
    initialFields: function () {

        CC.IdRol = decodeURIComponent(Utils.getQueryStringParameter('IDROL'));
        CC.IdEjec = decodeURIComponent(Utils.getQueryStringParameter('IDEJEC'));
        CC.Anio = decodeURIComponent(Utils.getQueryStringParameter('ANIO'));
        CC.Mes = decodeURIComponent(Utils.getQueryStringParameter('MES'));
        CC.TotCom = decodeURIComponent(Utils.getQueryStringParameter('TOTCOM'));
        if (CC.TotCom != undefined)
            CC.TotCom = CC.TotCom.replace(',', '.')
        $('#table-comisiones').bootstrapTable();
        SolicitarAprobacionCC.cargarAnios();

        $('#ddlMes').val(CC.Mes);
        $('#ddlAnio').val(CC.Anio);
        $('#btnBuscar').click(SolicitarAprobacionCC.buscarComisiones);

    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = 2020; i < year + 1; i++) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
    },
    CargarComisiones: function (data) {
        CC.DatosHistorial = data;
        var RolCurrent = CC.Roles.find(function (n) {
            return n.ID == CC.IdRol;
        });
        var item = {
            "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
            "RolId": CC.IdRol,
            "EjecutivoId": CC.IdEjec,
            "Anio": CC.Anio,
            "Mes": CC.Mes,
            "Estado": "Pendiente",
            "TotalComision": parseFloat(CC.TotCom).toFixed(2),
            "RolAprobador": RolCurrent.RolAprobador
        };

        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        var loader = true;
        if (CC.DatosHistorial <= 0) {
            if ($('#ddlAnio').val() == CC.Anio && $('#ddlMes').val() == CC.Mes) {
                funcSPO.addListItem(CC.ListaHistorial, item, SolicitarAprobacionCC.buscarComisiones, SolicitarAprobacionCC.errorSPO, '/cobranzas');
                loader = false;
            }
            table.bootstrapTable();
        }
        else {
            var _Item = CC.DatosHistorial[0];
            if (_Item.Estado != "Aprobado" && !CC.Actualizado && CC.Mes != "0" && $('#ddlAnio').val() == CC.Anio && $('#ddlMes').val() == CC.Mes) {

                CC.Actualizado = true;
                funcSPO.updateListItem(CC.ListaHistorial, item, _Item.ID, SolicitarAprobacionCC.buscarComisiones, SolicitarAprobacionCC.errorSPO, '/cobranzas');
                loader = false;
            }
            table.bootstrapTable({
                data: CC.DatosHistorial
            });
        };
        if (loader) {
            Utils.LoaderHide();
            Communica.Part.adjustSize();
        }
    },
    errorSPO: function (error) {
        Utils.swalError('Ocurrió un error: ' + error.message.value);
    },
    buscarComisiones: function () {

        Utils.LoaderShow();
        var filter = "EjecutivoId eq " + CC.IdEjec + " and Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(CC.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision&$expand=Ejecutivo", SolicitarAprobacionCC.CargarComisiones, null, true, '/cobranzas');
    },
    cargarRoles: function (data) {
        CC.Roles = data;
    }
}

$(document).ready(function () {
    SolicitarAprobacionCC.initial();
    Communica.Part.adjustSize();
    funcSPO.getCurrentUser();
    funcSPO.getListItems(CC.ListaRoles, SolicitarAprobacionCC.cargarRoles, null, true,'/cobranzas');
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