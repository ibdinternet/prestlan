var _pid = "";
function generateDialog(event, id) {
    _pid = id;

    $("[id$=tbComentario]").val('');

     //Fijamos la posición del diálogo junto al cursor
    $("#divDialog").dialog({
        width: 375,
        position: { my: 'left-60 top+20', at: 'bottom', of: event },
        autoOpen:true
    });
}

$(document).ready(function () {
    
    $("#bEnviar").click(function (e) {
        
        if ($("[id$=tbComentario]").val() != "") {
            $.ajax({
                type: "POST",
                url: "Servicios/DocumentService.asmx/sendReclamacion",
                data: '{"pid":"' + _pid + '", "contenido": "' + $("[id$=tbComentario]").val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var r = response.d;
                    if (response.d) {                        
                        operacionOK("reclenviadaok");
                    } else {
                        operacionError("ERROR: reclenviadanook");
                    }
                    $("#divDialog").dialog("close");
                },
                failure: function (msg) {
                    $("#divDialog").text(msg);
                }
            });
        }
    });

    $("[id$=GridView1] input[id$=chkAll]").click(function () {
        $("[id$=GridView1] input[id*=chkDocumento]").prop('checked', this.checked);
    });

    // Creamos el diálogo
    $("#divDialog").dialog({
        draggable: true,
        resizable: false,
        autoOpen: false,
        height: 'auto',
        modal: true
    });
});