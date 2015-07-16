var did = "";
var dialogOpener;
function generateDialog(event, id, v, u, f_id) {
    dialogOpener = event.currentTarget;
    did = id;
    // Fijamos la posición del diálogo junto al cursor
    $("#divDialog").dialog({
        position: { my: 'left-60 top+20', at: 'bottom', of: event }
    });

    // Obtenemos los datos del documento
    $.ajax({
        type: "POST",
        url: "Servicios/DocumentService.asmx/GetDocumentoById",
        data: '{"did":"' + did + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var doc = response.d;
            $("[id$=lblTitulo]").text(doc.Titulo);
            $("[id$=lblDescripcion]").text(doc.Descripcion);
            $("[id$=lblAutor]").text(doc.Autor);
            $("[id$=lblVersion]").text(v + ".0");
            $("[id$=lblEstado]").text(doc.EstadoDescripcion);
            if (doc.Etapa == "REVISION" || doc.Etapa == "RECHAZO")
            {
                $("[id$=lblEtapa]").text("(" + doc.EtapaDescripcion + ")");
            }
            else
            {
                $("[id$=lblEtapa]").text("");
            }
            
            // Gestión del estado
            switch (doc.Estado) {
                case "PUBLICADO":
                    $("#bEditar").show();
                    $("#bPasarABorrador").show();
                    $("#bPasarARevision").hide();
                    $(".bValidar").hide();
                    $("#bEliminarBorrador").hide();
                    break;
                case "CADUCADO":
                    $("#bEditar").show();
                    $("#bPasarABorrador").hide();
                    $("#bPasarARevision").hide();
                    $(".bValidar").hide();
                    $("#bEliminarBorrador").hide();
                    break;
                case "BORRADOR":
                    $("#bEditar").show();                    
                    if (doc.Etapa == "REVISION")
                    {
                        $("#bPasarARevision").hide();
                        $(".bValidar").hide();
                        $("#bPasarABorrador").show();
                        if (doc.PuedeValidar == true) {
                            $(".bValidar").show();
                        }
                        else {
                            $(".bValidar").hide();
                        }
                    }
                    else if (doc.Etapa == "RECHAZO")
                    {
                        $("#bPasarARevision").show();
                        $(".bValidar").hide();
                        $("#bPasarABorrador").hide();
                    }
                    else if (doc.Etapa == "BORRADOR")
                    {                        
                        $(".bValidar").hide();
                        $("#bPasarARevision").show();
                        $("#bEliminarBorrador").show();
                        $("#bPasarABorrador").hide();
                    }
                    break;
            }

            // Esto es dudoso FIXME
            if (!u) { // Desactivar botón de editar excepto para la última versión
                $("#bEditar").hide();
            }
            //Establezco el id del fichero para que pueda verlo
            $("#bVerFichero").data("fichero-id", f_id);

            // Abrimos el dialogo
            $("#divDialog").dialog("open");
        },
        failure: function (msg) {
            operacionError(msg);
        }
    });
}

$(document).ready(function () {
    // Creamos el diálogo
    $("#divDialog").dialog({
        draggable: true,
        resizable: false,
        autoOpen: false,
        height: 'auto',
        modal: true        
    });

    // Botón: Editar
    $("#bEditar").click(function (e) {
        window.location.href = 'documentos.aspx?mode=Edit&id=' + did;
    });
    $("#bEliminarBorrador").click(function (e) {
        $.ajax({
            type: "POST",
            url: "Servicios/DocumentService.asmx/eliminarBorrador",
            data: '{"did":"' + did + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var estado = response.d;
                if (estado == "OK") { // cambio de etapa OK. Mostrar mensaje 
                    location.reload();
                } else {
                    operacionError("error", "erroroperacion");
                }
            },
            failure: function (msg) {
                console.log("o%", dialogOpener);
                operacionError("error", "erroroperacion");
            }
        });
    });
    $("#bVerFichero").click(function (e) {
        window.open('handlers/opendoc.ashx?id=' + $(this).data("fichero-id"), '_blank');
    });
    // Botón: Pasar a borrador
    $("#bPasarABorrador").click(function (e) {
        $.ajax({
            type: "POST",
            url: "Servicios/DocumentService.asmx/pasarABorrador",
            data: '{"did":"' + did + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var estado = response.d;
                if (estado == "OK") { // cambio de etapa OK. Mostrar mensaje 
                    operacionOK("docaborrador");
                    $("#divDialog").dialog("close");
                    $(dialogOpener).prop("class", "borrador");
                } else {
                    operacionError("error", "erroroperacion");
                }
            },
            failure: function (msg) {
                console.log("o%", msg);
                operacionError("error", "erroroperacion");
            }
        });
    });

    // Botón: Pasar a revisión
    $("#bPasarARevision").click(function (e) {
        // Ajax para cambiar etapa del documento y emitir notificación a revisor
        $.ajax({
            type: "POST",
            url: "Servicios/DocumentService.asmx/pasarARevision",
            data: '{"did":"' + did + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var estado = response.d;
                if (estado == "OK") { // cambio de etapa OK. Mostrar mensaje 
                    operacionOK("docavalidar");
                    $("#divDialog").dialog("close");
                    $(dialogOpener).prop("class", "revision");
                } else {
                    operacionError("error", "erroroperacion");
                }
            },
            failure: function (msg) {
                operacionError("error", "erroroperacion");
            }
        });
    });

    // Botón: Pasar a revisión
    $("#bValidar").click(function (e) {
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
                    // Asignar clase de publicado a la versión de documento
                    $(dialogOpener).prop("class", "publicado");                 
                    operacionOK("docvalidado");
                    location.reload();
                } else {
                    operacionError("error", "erroroperacion");
                }
            },
            failure: function (msg) {
                operacionError("error", "erroroperacion");
            }
        });
    });

    $("[id$=GridView1] input[id$=chkAll]").click(function () {
        $("[id$=GridView1] input[id*=chkDocumento]").prop('checked', this.checked);
    });

    
});