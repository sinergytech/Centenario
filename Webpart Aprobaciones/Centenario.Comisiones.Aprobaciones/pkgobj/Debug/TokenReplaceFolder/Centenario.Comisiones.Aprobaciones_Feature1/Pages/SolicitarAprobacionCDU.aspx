<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title>Solicitar Aprobación</title>

    <link href="../Content/css/oneui.css" rel="stylesheet" />
    <link href="../Content/css/widgets.css" rel="stylesheet" />
    <link href="../Content/css/style.css" rel="stylesheet" />
    <link href="../Content/css/style_home.css" rel="stylesheet" />
    <script src="../Content/js/core/jquery.min.js"></script>

    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script type="text/javascript" src="/_layouts/15/SP.RequestExecutor.js"></script>

    <script src="../Content/table/bootstrap-table.js"></script>
    <link href="../Content/table/bootstrap-table.css" rel="stylesheet" />
    <script src="../Content/table/locale/bootstrap-table-es-ES.js"></script>

    <script src="../Content/js/core/bootstrap.min.js"></script>
    <script src="../Scripts/vendor/jqueryui/jquery-ui.min.js"></script>
    <script src="../Content/js/core/jquery.slimscroll.min.js"></script>
    <script src="../Content/js/core/jquery.scrollLock.min.js"></script>
    <script src="../Content/js/core/jquery.appear.min.js"></script>
    <script src="../Content/js/core/jquery.countTo.min.js"></script>
    <script src="../Content/js/core/jquery.placeholder.min.js"></script>
    <script src="../Content/js/core/js.cookie.min.js"></script>

    <!-- Page JS Plugins -->
    
    <script src="../Content/js/bootstrapsweetalert/sweetalert2.js"></script>
    <link href="../Content/js/bootstrapsweetalert/sweetalert2.css" rel="stylesheet" />

    <script src="../Content/js/plugins/bootstrap-datepicker/bootstrap-datepicker.min.js"></script>
    <script src="../Content/js/plugins/select2/select2.full.min.js"></script>
    <script src="../Content/js/plugins/masked-inputs/jquery.maskedinput.min.js"></script>
    <script src="../Content/js/plugins/dropzonejs/dropzone.min.js"></script>
    <script src="../Content/js/plugins/jquery-tags-input/jquery.tagsinput.min.js"></script>
    <script src="../Scripts/Comun/ResizeAppPart.js"></script>
    <script src="../Scripts/Comun/AlertJS.js"></script>
    <script src="../Scripts/Comun/jquery.mask.js"></script>
    <script src="../Scripts/Comun/moment.js"></script>
    <script src="../Scripts/Pages/Utils.js"></script>
    <script src="../Scripts/Pages/SPOHelper.js"></script>
    <script src="../Scripts/Pages/SolicitarAprobacionCDU.js"></script>
</head>
<body>
    <div id="cargando">
        <div>
            <div id="inner-wrap">
                <i class="fa fa-spinner fa-spin" style="font-size: 50px; color: rgba(248, 252, 254, 1)"></i>
            </div>
        </div>
    </div>
    <main class="main" role="main" id="content-AppPart">
        <div class="content">
            <div class="block">
                <div class="block-header">
                    <h3 class="block-title">Solicitudes de Aprobación</h3>
                </div>
                <div class="block-content">

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-1 control-label">Año</label>
                            <div class="col-md-1">
                                <select id="ddlAnio" class="form-control" size="1">
                                </select>
                            </div>
                            <label class="col-md-1 control-label">Mes</label>
                            <div class="col-md-2">
                                <select id="ddlMes" class="form-control" size="1">
                                    <option value="0">Todos</option>
                                    <option value="1">Enero</option>
                                    <option value="2">Febrero</option>
                                    <option value="3">Marzo</option>
                                    <option value="4">Abril</option>
                                    <option value="5">Mayo</option>
                                    <option value="6">Junio</option>
                                    <option value="7">Julio</option>
                                    <option value="8">Agosto</option>
                                    <option value="9">Setiembre</option>
                                    <option value="10">Octubre</option>
                                    <option value="11">Noviembre</option>
                                    <option value="12">Diciembre</option>
                                </select>
                            </div>
                            <div class="col-sm-4 col-lg-4">
                                <button class="btn btn-minw btn-rojo" id="btnBuscar" type="button">Buscar</button>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table id="table-comisiones"
                            class="table table-striped table-vcenter"
                            data-search="true"
                            data-show-refresh="true"
                            data-show-toggle="true"
                            data-toggle="table"
                            <%--data-pagination="true"
                            data-page-list="[20]"
                            data-page-size="20"--%>
                            data-mobile-responsive="true"
                            data-locale="es-SP">
                            <thead>
                                <tr>
                                    <th class="w-10" data-field="Ejecutivo.Title" data-align="left">Ejecutivo</th>
                                    <th class="w-50" data-field="Anio" data-align="center">Año</th>
                                    <th class="w-10" data-field="Mes" data-align="center" data-formatter="tableFormats.FormatMes">Mes</th>
                                    <th class="w-10" data-field="TotalComision" data-align="right" data-formatter="tableFormats.FormatDecimal">Total Comisión</th>
                                    <th class="w-10" data-field="Estado" data-align="center">Estado</th>
                                </tr>
                            </thead>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </main>
</body>
</html>
