var Rentas = {};
var SolicitarAprobacionRentas = {
    initial: function () {
        Rentas.ListaHistorial = 'Historial Aprobaciones';
        Rentas.ListaRoles = 'Roles';
        Rentas.Actualizado = false;
        SolicitarAprobacionRentas.initialFields();
    },
    initialFields: function () {

        Rentas.IdRol = decodeURIComponent(Utils.getQueryStringParameter('IDROL'));
        Rentas.IdEjec = decodeURIComponent(Utils.getQueryStringParameter('IDEJEC'));
        Rentas.Anio = decodeURIComponent(Utils.getQueryStringParameter('ANIO'));
        Rentas.Mes = decodeURIComponent(Utils.getQueryStringParameter('MES'));
        Rentas.TotCom = decodeURIComponent(Utils.getQueryStringParameter('TOTCOM'));
        if (Rentas.TotCom != undefined)
            Rentas.TotCom = Rentas.TotCom.replace(',', '.')
        Rentas.Concepto = decodeURIComponent(Utils.getQueryStringParameter('CONCEP'));
        if (Rentas.Concepto=='CONCEPValue') {
            Rentas.Concepto = '';
        }

        $('#table-comisiones').bootstrapTable();
        SolicitarAprobacionRentas.cargarAnios();

        $('#ddlMes').val(Rentas.Mes);
        $('#ddlAnio').val(Rentas.Anio);
        $('#btnBuscar').click(SolicitarAprobacionRentas.buscarComisiones);

    },
    cargarAnios: function () {
        var year = moment().year();
        for (var i = 2020; i < year + 1; i++) {
            $('#ddlAnio').append('<option value="' + i + '">' + i + '</option>');
        }
    },
    CargarComisiones: function (data) {
        Rentas.DatosHistorial = data;
        var RolCurrent = Rentas.Roles.find(function (n) {
            return n.ID == Rentas.IdRol;
        });
        var item = {
            "__metadata": { "type": "SP.Data.HistorialAprobacionesListItem" },
            "RolId": Rentas.IdRol,
            "EjecutivoId": Rentas.IdEjec,
            "Anio": Rentas.Anio,
            "Mes": Rentas.Mes,
            "Estado": "Pendiente",
            "TotalComision": parseFloat(Rentas.TotCom).toFixed(2),
            "Concepto": Rentas.Concepto,
            "RolAprobador": RolCurrent.RolAprobador
        };

        var table = $("#table-comisiones");
        table.bootstrapTable("destroy");
        var loader = true;
        var lstFiltrado = []
        switch (RolCurrent.Codigo) {
            case '004':
                lstFiltrado = Rentas.DatosHistorial.filter(x => x.Anio == Rentas.Anio && x.Mes == Rentas.Mes && x.TotalComision == parseFloat(Rentas.TotCom).toFixed(2));
                break;
            case '006':
                lstFiltrado = Rentas.DatosHistorial.filter(x => x.Anio == Rentas.Anio && x.Mes == Rentas.Mes && x.TotalComision == parseFloat(Rentas.TotCom).toFixed(2) && x.Concepto == Rentas.Concepto);
                break;
            default:
                lstFiltrado = Rentas.DatosHistorial.filter(x => x.Anio == Rentas.Anio && x.Mes == Rentas.Mes && x.TotalComision == parseFloat(Rentas.TotCom).toFixed(2));
                break;
        }
        if (lstFiltrado.length == 0)
        {
            if ($('#ddlAnio').val() == Rentas.Anio && $('#ddlMes').val() == Rentas.Mes && Rentas.TotCom != '') {
                funcSPO.addListItem(Rentas.ListaHistorial, item, SolicitarAprobacionRentas.buscarComisiones, SolicitarAprobacionRentas.errorSPO, '/rentas');
                loader = false;
            }
            table.bootstrapTable();
        }
        else {
            if (!Rentas.Actualizado && Rentas.Mes != "0" && $('#ddlAnio').val() == Rentas.Anio && $('#ddlMes').val() == Rentas.Mes && Rentas.TotCom != '') {
                var idItem = lstFiltrado[0].ID;
                Rentas.Actualizado = true;
                loader = false;
                funcSPO.updateListItem(Rentas.ListaHistorial, item, idItem, SolicitarAprobacionRentas.buscarComisiones, SolicitarAprobacionRentas.errorSPO, '/rentas');
            }
            table.bootstrapTable({
                data: Rentas.DatosHistorial
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
        var filter = "EjecutivoId eq " + Rentas.IdEjec + " and Anio eq " + $('#ddlAnio').val();
        if ($('#ddlMes').val() != "0")
            filter = filter + " and Mes eq " + $('#ddlMes').val();
        funcSPO.getListItemsFilter(Rentas.ListaHistorial, filter + "&$select=ID,Ejecutivo/ID,Ejecutivo/Title,Ejecutivo/EMail,Anio,Mes,Estado,TotalComision,Concepto&$expand=Ejecutivo", SolicitarAprobacionRentas.CargarComisiones, null, true, '/rentas');
    },
    cargarRoles: function (data) {
        Rentas.Roles = data;
    }
}

$(document).ready(function () {
    SolicitarAprobacionRentas.initial();
    Communica.Part.adjustSize();
    funcSPO.getCurrentUser();
    funcSPO.getListItems(Rentas.ListaRoles, SolicitarAprobacionRentas.cargarRoles, null, true, '/rentas');
    SolicitarAprobacionRentas.buscarComisiones();
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