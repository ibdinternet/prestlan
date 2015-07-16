$(document).ready(function () {

    // Multiselect de actividades
    $("[id$=lbActividadesTrabajador]").multiselect({
        checkAllText: "Todas",
        uncheckAllText: "Ninguna",
        selectedText: "# actividades(s)",
        noneSelectedText: "Seleccione actividad(s)",
    });    

    //function chequearTipoTrabajador() {
    //    var rbt = $("[id$=rbTrabajador] input:radio:checked").val();
    //    if (rbt == "1") {
    //        $("[id$=ddEmpresa]").prop("disabled", true);
    //        $("[id$=rfvEmpresa]").prop("disabled", true);
    //    } else {
    //        $("[id$=ddEmpresa]").prop("disabled", false);
    //        $("[id$=rfvEmpresa]").prop("disabled", false);
    //    }
    //}

    //// Procesar radiobuttontrabajador
    //$("[id$=rbTrabajador]").click(function () {
    //    chequearTipoTrabajador();
    //});

    //chequearTipoTrabajador();

});