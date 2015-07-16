
$(document).ready(function () {
    $(".jsBotoneraRequisito").each(function (index, element) {
        var row = $(element).parents("tr")[0];
        var td = $(row).find("td");
        $(td).attr("colspan", 2);
        var id = parseInt($(element).data("requisito-id").toString());
        if (id > 0)
        {
            var botones = "";
            botones = botones + "<a href='/requisitos.aspx?mode=Edit&id=" + id + "'><span class='icon-editar'></span></a>";
            botones = botones + "&nbsp;";
            botones = botones + "<a class='btnEliminarRequisito'><span class='icon-cancelar-small'></span></a>";
            $(element).html(botones);
        }
    });

    $(".btnEliminarRequisito").click(function () {
        if (confirm('¿Está seguro de que quiere eliminar este requisito?'))
        {
            var req = $(this).parents(".jsBotoneraRequisito")[0];
            var id = $(req).data("requisito-id");
            __doPostBack("EliminarRequisito", id)
        }
    });
});