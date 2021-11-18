/*Preloader*/
/*$(window).on('load', function () {
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 200);
    setInterval(function () {
        if ($(".ms-WPBody").length) {
            $(".ms-WPBody").css({ "height": "100%!Important" });
        }
    }, 100);

});*/

$(function () {

    "use strict";


    /*var window_width = $(window).width();

    $('.datepicker').pickadate({
        monthsFull: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Set', 'Oct', 'Nov', 'Dic'],
        weekdaysFull: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
        selectMonths: true,
        selectYears: 100, // Puedes cambiarlo para mostrar más o menos años
        today: 'Hoy',
        clear: 'Limpiar',
        close: 'Ok',
        labelMonthNext: 'Siguiente mes',
        labelMonthPrev: 'Mes anterior',
        labelMonthSelect: 'Selecciona un mes',
        labelYearSelect: 'Selecciona un año',
    });

    $('.tabs').tabs();*/
    $('.modal').modal();
    /*
    // Check first if any of the task is checked
    $('#task-card input:checkbox').each(function () {
        checkbox_check(this);
    });

    // Task check box
    $('#task-card input:checkbox').change(function () {
        checkbox_check(this);
    });

    // Check Uncheck function
    function checkbox_check(el) {
        if (!$(el).is(':checked')) {
            $(el).next().css('text-decoration', 'none'); // or addClass
        } else {
            $(el).next().css('text-decoration', 'line-through'); //or addClass
        }
    }

    // Set checkbox on forms.html to indeterminate
    var indeterminateCheckbox = document.getElementById('indeterminate-checkbox');
    if (indeterminateCheckbox !== null)
        indeterminateCheckbox.indeterminate = true;

    // Commom, Translation & Horizontal Dropdown
    $('.dropdown-button, .translation-button, .dropdown-menu').dropdown({
        inDuration: 300,
        outDuration: 225,
        constrainWidth: false,
        hover: true,
        gutter: 0,
        belowOrigin: true,
        alignment: 'left',
        stopPropagation: false
    });

    // Materialize scrollSpy
    $('.scrollspy').scrollSpy();

    // Materialize tooltip
    $('.tooltipped').tooltip({
        delay: 50
    });

    //Main Right Sidebar Menu
    $('.sidebar-collapse').sideNav({
        edge: 'left', // Choose the horizontal origin
    });

    // Overlay Menu (Full screen menu)
    $('.menu-sidebar-collapse').sideNav({
        menuWidth: 240,
        edge: 'left', // Choose the horizontal origin
        //closeOnClick:true, // Set if default menu open is true
        menuOut: false // Set if default menu open is true
    });*/

    //Main Left Sidebar Chat
    $('.filters-collapse').sideNav({
        menuWidth: 400,
        edge: 'right',
    });

    // Perfect Scrollbar
    /*var leftnav = $(".page-topbar").height();
    var leftnavHeight = window.innerHeight - leftnav;
    if (!$('#slide-out.leftside-navigation').hasClass('native-scroll')) {
        $('.leftside-navigation').perfectScrollbar({
            suppressScrollX: true
        });
    }
    var righttnav = $("#chat-out").height();
    $('.rightside-navigation').perfectScrollbar({
        suppressScrollX: true
    });

    // Toggle Flow Text
    var toggleFlowTextButton = $('#flow-toggle')
    toggleFlowTextButton.click(function () {
        $('#flow-text-demo').children('p').each(function () {
            $(this).toggleClass('flow-text');
        })
    });

    // Detect touch screen and enable scrollbar if necessary
    function is_touch_device() {
        try {
            document.createEvent("TouchEvent");
            return true;
        } catch (e) {
            return false;
        }
    }
    if (is_touch_device()) {
        $('#nav-mobile').css({
            overflow: 'auto'
        })
    }*/
});
