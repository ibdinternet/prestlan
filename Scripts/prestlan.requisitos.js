
$(document).ready(function () {
    // Chosen
    //$('[id$=lbTipoDocumentoRequisito]').chosen({ placeholder_text_multiple: 'Seleccione tipos de documentos', width: "350px" });
    $("[id$=lbTipoDocumentoRequisito]").multiselect({
        checkAllText: "Todos",
        uncheckAllText: "Ninguno",
        selectedText: "# Tipos documento(s)",
        noneSelectedText: "Seleccione tipos de documento(s)",
    });
});