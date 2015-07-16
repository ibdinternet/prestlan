$(document).ready(function () {
    $("[id$=InsertButton]").click(function (e) {
        var esUnico = false;
        $.ajax({
            type: "POST",
            async: false,
            url: "Servicios/UsuarioService.asmx/EsEmailUnico",
            data: '{"Email":"' + $("[id$=tbEmail]").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                esUnico = response.d;
            },
            failure: function (msg) {
                esUnico = false;
            }
        });
        if (esUnico) {
            return true;
        } else {
            // operacionError("Error: el email especificado ya está en uso.");
            operacionError("ERROR: erroremail");
            return false;
        }
    });

});