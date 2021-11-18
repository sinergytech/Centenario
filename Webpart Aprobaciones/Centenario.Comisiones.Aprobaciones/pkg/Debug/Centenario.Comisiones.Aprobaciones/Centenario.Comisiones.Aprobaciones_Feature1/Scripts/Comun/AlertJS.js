function alertBase(type, title, body, Callback, timer, Callcancel) {
    var modalAlert = 'modalAlert-' + Math.floor((Math.random() * 100) + 1);
    var alertDiv = document.createElement('div');
    alertDiv.id = modalAlert;
    alertDiv.className = 'modal fade modal-alert';
    document.getElementsByTagName('body')[0].appendChild(alertDiv);
    $("#" + modalAlert).html("<div class='modal-dialog'><div class='modal-content'><div class='modal-header'></div><div class='modal-title'></div><div class='modal-body'></div><div class='modal-footer'></div></div><div class='modal-progress'></div></div>");
    var classType = "", iconType = "", classButtons = "", buttonsType = "<button type='button' class='btn color1 btn-ok' data-dismiss='modal'>Aceptar</button>", progressType = "";
    switch (type) {
        case 'warning':
            classType = "modal-warning in";
            iconType = "<i class='fa fa-exclamation-triangle'></i>";
            progressType = "progress-bar-warning";
            classButtons = "btn-warning";
            break;
        case 'error':
            classType = "modal-danger in";
            iconType = "<i class='fa fa-times-circle'></i>";
            progressType = "progress-bar-danger";
            classButtons = "btn-danger";
            break;
        case 'success':
            classType = "modal-success in";
            iconType = "<i class='fa fa-check-circle'></i>";
            progressType = "progress-bar-success";
            classButtons = "btn-success";
            break;
        case 'info':
            classType = "modal-info in";
            iconType = "<i class='fa fa-info-circle'></i>";
            progressType = "progress-bar-info";
            classButtons = "btn-info";
            break;
        case 'confirm':
            classType = "modal-info in";
            iconType = "<i class='fa fa-question-circle'></i>";
            buttonsType = "<button type='button' class='btn color1 btn-confirm'>Aceptar</button><button type='button' class='btn color2 btn-cancel' data-dismiss='modal'>Cancelar</button>";
            classButtons = "btn-success";
            break;
        case 'redirect':
            classType = "modal-warning in";
            iconType = "<i class='fa fa-exclamation-triangle'></i>";
            progressType = "progress-bar-warning";
            buttonsType = "<button type='button' class='btn color1 btn-confirm'>Actualizar</button><button type='button' class='btn color2 btn-cancel' data-dismiss='modal'>Cancelar</button>";
            classButtons = "btn-success";
            break;
        default:
            return false;
    }
    $("#" + modalAlert).addClass(classType);
    $("#" + modalAlert + " .modal-header").html(iconType);
    $("#" + modalAlert + " .modal-title").html(title);
    $("#" + modalAlert + " .modal-body").html(body);
    $("#" + modalAlert + " .modal-footer").html(buttonsType);
    $("#" + modalAlert + " .btn-ok").addClass(classButtons);
    if (type == 'confirm') {
        $("#" + modalAlert + " button.btn-cancel").on("click", function (e) {
            $("#" + modalAlert).remove();
            window.parent.postMessage("alertClose", "*");
            Callback(false);
            Callcancel(true);
        });
        $("#" + modalAlert + " button.btn-confirm").on("click", function (e) {
            $("#" + modalAlert).remove();
            window.parent.postMessage("alertClose", "*");
            Callback(true);
            Callcancel(false);
        });
    } else if (type == 'redirect') {
        $("#" + modalAlert + " button.btn-cancel").on("click", function (e) {
            $("#" + modalAlert).remove();
            window.parent.postMessage("alertClose", "*");
            Callback(false);
        });
        $("#" + modalAlert + " button.btn-confirm").on("click", function (e) {
            $("#" + modalAlert).remove();
            window.parent.postMessage("alertClose", "*");
            Callback(true);
        });
    } else {
        $("#" + modalAlert + " button.btn-ok").on("click", function (e) {
            $("#" + modalAlert).remove();
            window.parent.postMessage("alertClose", "*");
            if (Callback != undefined) {
                Callback(true);
            }
        });
    }
    if (timer != undefined && type != 'confirm') {
        $("#" + modalAlert + " .modal-progress").html("<div class='progress'><div class='progress-bar " + progressType + " progress-bar-striped active' style='width: 100%'></div></div>");
        setInterval(function () {
            if ($("#" + modalAlert + " .progress-bar").width() == 0) {
                $("#" + modalAlert + " button.btn-ok").click();
            } else {
                $("#" + modalAlert + " .progress-bar").width($("#" + modalAlert + " .progress-bar").width() - 200);
            }
        }, timer);
    }
}