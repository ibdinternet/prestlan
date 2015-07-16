using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Prestlan.Servicios {
    /// <summary>
    /// Descripción breve de UsuarioService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class UsuarioService : System.Web.Services.WebService {

        [WebMethod]
        public bool EsEmailUnico(string Email) {
            return Utilidades.Autentificacion.EsEmailUnico(Email);
         
        }
    }
}
