// Función para abrir documentos en ventana nueva
function openDoc(id) {
    $.ajax({
        type: "POST",
        url: "handlers/opendoc.ashx",
        contentType: "application/json; charset=utf-8",
        success: OnComplete,
        error: OnFail
    });
}

function selectRB(rb) {            
    $('[id$=GridView1]').find('input:radio').each(function () {        
        $(this).attr('checked', false);
    });
    rb.checked = true;
}

function obtenerNombreValidador(did) {
    $.ajax({
        type: "POST",
        url: "Servicios/DocumentService.asmx/obtenerNombreUsuarioRevisor",
        data: '{"did":"' + did + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var estado = response.d;
            if (estado.estado == "OK") { 
                $('#docRevisor_' + did).html(estado.mensaje);
            } else {
                operacionError(estado.mensaje);
            }
        },
        failure: function (msg) {
            operacionError("error", "erroroperacion");
        }
    });
}


$(document).ready(function () {    

    //Creamos el diálogo
    $("[id$=divDialog]").dialog({
        draggable: true,
        resizable: false,
        autoOpen: false,
        height: 'auto',
        modal: true
    });
    
    // Botón rechazar
    $("#bRechazarDocumento").click(function (e) {
        var did;
        // Localizar check y obtener did
        $('[id$=GridView1]').find('input:radio').each(function () {
            if ($('input:radio[name=\'' + $(this).attr('name') + '\']:checked').length) {
                // Chequeado
                var parentTD = $('input:radio[name=\'' + $(this).attr('name') + '\']:checked').parents('td')[0];
                did = $('input:hidden', parentTD).val();
            }
        });
        // Ajax para cambiar etapa del documento y emitir notificación 
        $.ajax({
            type: "POST",
            url: "Servicios/DocumentService.asmx/pasarARechazado",
            data: '{"did":"' + did + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var estado = response.d;
                if (estado == "OK") { // cambio de etapa OK. Mostrar mensaje 
                    //operacionOK("docrechazado");
                    location.reload();
                } else {
                    operacionError(estado);
                }
            },
            failure: function (msg) {
                operacionError("error", "erroroperacion");
            }
        });
    });
    
    // Botón validar
    $("#bValidarDocumento").click(function (e) {
        var did;
        // Localizar check y obtener did
        $('[id$=GridView1]').find('input:radio').each(function () {            
            if ($('input:radio[name=\'' + $(this).attr('name') + '\']:checked').length) {
                // Chequeado
                var parentTD = $('input:radio[name=\'' + $(this).attr('name') + '\']:checked').parents('td')[0];
                did = $('input:hidden', parentTD).val();
            } 
        });
        
        // Ajax para cambiar etapa del documento y emitir notificación 
        $.ajax({
            type: "POST",
            url: "Servicios/DocumentService.asmx/pasarAPublicado",
            data: '{"did":"' + did + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var estado = response.d;
                if (estado == "OK") { // cambio de etapa OK. Mostrar mensaje                                                             
                    //// Asignar clase de publicado a la versión de documento
                    //$('#docVersion_' + did).attr('class', 'publicado');
                    //obtenerNombreValidador(did);                    
                    //operacionOK("docvalidado");
                    location.reload();
                } else {
                    operacionError(estado);
                }
            },
            failure: function (msg) {
                operacionError("error", "erroroperacion");
            }
        });
    });        
});