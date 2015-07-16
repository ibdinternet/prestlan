var dialog_busquedas, dialog_notificaciones;

function formBuscar() {
    // Obtener empleados
    var empleados = [];
    $('[id$=lbEmpleados] :selected').each(function (i, selected) {
        empleados[i] = selected.value;
    });

    // Obtener empresas
    var empresas = [];
    $('[id$=lbEmpresas] :selected').each(function (i, selected) {
        empresas[i] = selected.value;
    });

    // Obtener actividades
    var actividades = [];
    $('[id$=lbActividades] :selected').each(function (i, selected) {
        actividades[i] = selected.value;
    });

    // Obtener tipos de documentos
    var tipodocumentos = [];
    $('[id$=lbTipoDocumentos] :selected').each(function (i, selected) {
        tipodocumentos[i] = selected.value;
    });

    var url = 'list_documentos.aspx?q=s';

    if (empleados.length > 0) url += '&e=' + empleados.join();
    if (empresas.length > 0) url += '&em=' + empresas.join();
    if (actividades.length > 0) url += '&a=' + actividades.join();
    if (tipodocumentos.length > 0) url += '&td=' + tipodocumentos.join();

    window.location.assign(url);

}

// Notificaciones generales
function operacionError(tipo, estado) {    
    traducciones = [];
    traducciones[0] = "erroroperacion";
    traducciones[1] = estado;
    $.ajax({
        type: "POST",
        async: false,
        url: "Servicios/TraduccionesService.asmx/getTG",
        data: '{"traducciones":' + JSON.stringify(traducciones) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            new PNotify({
                title: response.d[0],
                text: response.d[1],
                type: tipo,
                animation: 'show',
                before_open: function (PNotify) {
                    PNotify.get().css({ top: 125, right: 50 });
                }
            });
        },
        failure: function (msg) {
            
        }
    });    
}

function operacionError(estado) {
    var tipo = "error";    

    traducciones = [];
    traducciones[0] = "erroroperacion";
    
    $.ajax({
        type: "POST",
        async: false,
        url: "Servicios/TraduccionesService.asmx/getTG",
        data: '{"traducciones":' + JSON.stringify(traducciones) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            new PNotify({
                title: response.d[0],
                text: estado,
                type: tipo,
                animation: 'show',
                before_open: function (PNotify) {
                    PNotify.get().css({ top: 125, right: 50 });
                }
            });
        },
        failure: function (msg) {

        }
    });
}

function operacionOK(estado) {
    traducciones = [];
    traducciones[0] = "okoperacion";
    traducciones[1] = estado;
    $.ajax({
        type: "POST",
        async: false,
        url: "Servicios/TraduccionesService.asmx/getTG",
        data: '{"traducciones":' + JSON.stringify(traducciones) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            new PNotify({
                title: response.d[0],
                text: response.d[1],
                type: 'success',
                animation: 'show',
                before_open: function (PNotify) {
                    PNotify.get().css({ top: 125, right: 50 });
                }
            });            
        },
        failure: function (msg) {

        }
    });

    
}

var _strBuscar = "";
var _strCerrar = "";
var _strSeleccioneempleados = "";
var _strSeleccioneempresas = "";
var _strSeleccioneactividades = "";
var _strSeleccionetiposdedocumentos = "";


$(document).ready(function () {
    $("#noti_list").on("click", ".noti_item", function (e) {
        var id = $(e.target).find(".noti_id").text();
        window.location.href = "documentos.aspx?mode=Edit&id=" + id;
    });
    $("#noti_list_pagina").on("click", ".noti_item", function (e) {
        var id = $(e.target).find(".noti_id").text();
        window.location.href = "documentos.aspx?mode=Edit&id=" + id;
    });
    // Obtener traducciones botonera buscador
    traducciones = [];
    traducciones[0] = "buscar";
    traducciones[1] = "cerrar";
    traducciones[2] = "seleccioneempleados";
    traducciones[3] = "seleccioneempresas";
    traducciones[4] = "seleccioneactividades";
    traducciones[5] = "seleccionetiposdocumentos";
    traducciones[6] = "seleccioneestado";
    $.ajax({
        type: "POST",
        async: false,
        url: "Servicios/TraduccionesService.asmx/getTG",
        data: '{"traducciones":' + JSON.stringify(traducciones) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            _strBuscar = response.d[0];
            _strCerrar = response.d[1];
            _strSeleccioneempleados = response.d[2];
            _strSeleccioneempresas = response.d[3];
            _strSeleccioneactividades = response.d[4];
            _strSeleccionetiposdedocumentos = response.d[5];
            _strSeleccioneestados = response.d[6];
        },
        failure: function (msg) {

        }
    });

    // Inicialización de notificaciones
    PNotify.prototype.options.styling = "jqueryui";

    // Diálogo para las notificaciones
    dialog_notificaciones = $("#wrapper_notificaciones").dialog({
        draggable: false,
        resizable: false,
        autoOpen: false,
        height: 365,
        width: 400,
        modal: false,
        closeOnEscape: true,
        open: function () {
            $("#wrapper_notificaciones").css("visibility", "visible");
            ObtenerNotificaciones("#noti_list");
        },
        show: {
            effect: "fade",
            duration: 150
        },
        hide: {
            effect: "fade",
            duration: 150
        },
        clickOutside: true,
        clickOutsideTrigger: "#show_notificaciones"
    });


    $('#show_notificaciones').click(function (event) {
        if (dialog_notificaciones.dialog("isOpen")) dialog_notificaciones.dialog("close");
        else {
            dialog_notificaciones.dialog({
                position: { my: 'left-70 top+20', at: 'bottom', of: event, collision: 'none' }
            });                        
            dialog_notificaciones.dialog("open");
        }
    });


    // Diálogo para las búsquedas
    dialog_busquedas = $("#divDialogFormSearch").dialog({
        draggable: false,
        resizable: true,
        autoOpen: false,
        height: 'auto',
        width: '500px',
        modal: false,
        closeOnEscape: true,
        show: {
            effect: "fade",
            duration: 150
        },
        hide: {
            effect: "fade",
            duration: 150
        },
        buttons: [
            {
                text: _strBuscar,
                click: formBuscar
            },
            {
                text: _strCerrar,
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],        
        open: function (event, ui) {
            $("#divDialogFormSearch").css("visibility", "visible");
            $('[id$=lbEmpleados]').blur();
        },
        clickOutside: true,
        clickOutsideTrigger: "#show_buscador"
    });

    $("#show_buscador").click(function (event) {
        if (dialog_busquedas.dialog("isOpen")) dialog_busquedas.dialog("close");
        else {
            dialog_busquedas.dialog({
                position: { my: 'left-60 top+20', at: 'bottom', of: event }
            });            
            dialog_busquedas.dialog("open");
        }
    });
    $('[id$=lbEmpleados]').each(function (index, element) {
        var groups = {};
        $(element).find("option:contains('[@*separador*@]')").each(function (i, e) {
            var indexSeparador = $(e).text().indexOf('[@*separador*@]');
            var empresa = $(e).text().substring(indexSeparador + 15);
            $(e).attr("data-empresa", empresa);
            var trabajador = $(e).text().substring(0, indexSeparador);
            $(e).text(trabajador);
            groups[$.trim(empresa)] = true;
        });
        $.each(groups, function (c) {
            $(element).find("option[data-empresa='" + c + "']").wrapAll('<optgroup label="' + c + '">');
        });
    });

    $('[id$=lbEmpresas]').chosen({ placeholder_text_multiple: _strSeleccioneempresas });
    $('[id$=lbEmpleados]').chosen({ placeholder_text_multiple: _strSeleccioneempleados});    
    $('[id$=lbActividades]').chosen({ placeholder_text_multiple: _strSeleccioneactividades });
    $('[id$=lbTipoDocumentos]').chosen({ placeholder_text_multiple: _strSeleccionetiposdedocumentos });
    $('[id$=lbEstados]').chosen({ placeholder_text_multiple: _strSeleccioneestados});
    //// Cerrar notificaciones on click
    //$('.icon-cancel-circle').on("click", function () {
    //    var id = $(this).parent('div').find(".noti_id"); // Obtenemos el id de la notificación cerrada para marcarla como léida cuando corresponda
    //    $(this).parent('div').fadeOut();       // Cerrar la notificación 
    //});

    // Actualizar contador de notificaciones
    $.ajax({
        type: "POST",
        url: "Servicios/NotificacionesService.asmx/getNumeroNotificaciones",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var n = response.d;
            $("#contador_notificaciones").text(n);
            $("#contador_notificaciones2").text(n);
        },
        failure: function (msg) {
            $("#contador_notificaciones").text('0');
            $("#contador_notificaciones2").text('0');
        }
    });
});

function ObtenerNotificaciones(containerSelector) {
    // Obtener notificaciones
    $.ajax({
        type: "POST",
        url: "Servicios/NotificacionesService.asmx/getNotificaciones",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (lista) {
            var o = "";
            for (i = 0; i < lista.d.length; i++) {
                o += "<div class='noti_item'>";
                o += "<div class='noti_id'>" + lista.d[i].Documento_Id + "</div>";
                o += lista.d[i].Descripcion;
                //o += "<span class='icon-cancel-circle'></span>";
                o += "</div>";
            }
            $(containerSelector).html(o);
        },
        failure: function (msg) {
            $(containerSelector).html("");
        }
    });
}