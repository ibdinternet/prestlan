using IBD.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Prestlan.Servicios {
    /// <summary>
    /// Descripción breve de TraduccionesService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class TraduccionesService : System.Web.Services.WebService {

        [WebMethod(EnableSession = true)]
        public string[] getTG(string[] traducciones) {
            List<string> traducido = new List<string>();
            foreach (string t in traducciones) {
                traducido.Add(Traduce.getTG(t));                
            }
            return traducido.ToArray();
        }        
    }
}
