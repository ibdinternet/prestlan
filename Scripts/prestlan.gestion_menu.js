var currentLi = (function () {
    var pathArray = window.location.pathname.toLowerCase().split('/');
    var p = pathArray[pathArray.length - 1];
    p = p.replace("list_", "");
    if (p.substr(0, 4) == "adm_") return "li_administracion";
    return "li_" + p.substr(0, p.length - 5);
}());
$(document).ready(function () {    
    $("#main-nav li#" + currentLi).addClass("expand");
});