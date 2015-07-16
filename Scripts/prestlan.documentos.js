$(document).ready(function () {
    $(".jsSelectEmpresa").chosen();
    $(".jsSelectTipoDocumento").chosen();

    // Multiselect de asignación
    $("[id$=ddAsignacion]").multiselect({
        checkAllText: "Todos",
        uncheckAllText: "Ninguno",
        selectedText: "# propietario(s)",
        noneSelectedText: "Seleccione propietario(s)",
    });


    $("[id$=tbFechaCaducidad]").datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: 'images/calendar.gif',
        disabled: true
    });

    function gestionarCaducidad() {
        if ($("[id$=cbCaducidad]").prop('checked')) {
            $("[id$=tbFechaCaducidad]").prop("disabled", false);
            $("[id$=tbFechaCaducidad]").datepicker('enable');
        } else {
            $("[id$=tbFechaCaducidad]").prop("disabled", true);
            $("[id$=tbFechaCaducidad]").datepicker('disable');
        }
    }

    $("[id$=cbCaducidad]").click(function () {
        gestionarCaducidad();
    });

    // Inicializar caducidad
    gestionarCaducidad();


});