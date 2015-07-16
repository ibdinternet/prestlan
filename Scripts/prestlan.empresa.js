$(function () {
    $("[id$=tbfechaAlta]").datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: 'images/calendar.gif'
    });

    $("[id$=fechaAltaTextBox]").datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: 'images/calendar.gif'
    });

    $("[id$=tbfechaBaja]").datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: 'images/calendar.gif'
    });

    $("[id$=fechaBajaTextBox]").datepicker({
        dateFormat: 'dd/mm/yy',
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: 'images/calendar.gif'
    });
});