using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Prestlan.Utilidades {
    public class Autentificacion {

        protected static Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        /// <summary>
        /// Comprueba las credenciales de usuario
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int Autenticar(string email, string password) {
            int Usuario_Id = 0;

            var usuario = _db.Usuario.Where(u => u.email == email);
            if (usuario.Count() > 1) return 0; // Hay más de un usuario con el mismo correo, ERROR!!
            if (usuario.Count() == 0) return 0; // No hay usuarios con ese email
            bool validacion_clave = Hashing.ValidatePassword(password, usuario.FirstOrDefault().clave);
            if (validacion_clave) {
                Usuario_Id = usuario.FirstOrDefault().Id;
            }
            return Usuario_Id;
        }

        /// <summary>
        /// Determina si un email es único en la tabla de usuarios
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EsEmailUnico(string email) {
            if (email == "" || _db.Usuario.FirstOrDefault(x => x.email == email) != null) return false;
            else return true;
        }

        /// <summary>
        /// Crea una contraseña bien encriptada
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        public static string CrearPassword(string clave) {
            String passwordHash = "";
            if (clave != "") {
                passwordHash = BCrypt.Net.BCrypt.HashPassword(clave, BCrypt.Net.BCrypt.GenerateSalt(12));
            }
            return passwordHash;
        }

        /// <summary>
        /// Obtiene el ID del usuario logeado a partir del ID almacenado en la sesión
        /// </summary>
        /// <returns></returns>
        public static int GetUsuarioID() {
            if (HttpContext.Current.Session["Usuario_Id"] != null) {
                return Convert.ToInt32(HttpContext.Current.Session["Usuario_Id"].ToString());
            } else {                
                return 0;
            } 
        }

        /// <summary>
        /// Obtiene el id de la empresa actual
        /// </summary>
        /// <returns></returns>
        public static int GetEmpresaID() {
            return _db.Configuracion.FirstOrDefault().id;
        }

        /// <summary>
        /// Obtiene el email de un usuario
        /// </summary>
        /// <param name="Usuario_Id"></param>
        /// <returns></returns>
        public static string GetUsuarioEmail(int Usuario_Id) {
            return _db.Usuario.Find(Usuario_Id).email;
        }

        /// <summary>
        /// Obtiene el rol de un usuario a partir de su ID
        /// </summary>
        /// <param name="Usuario_Id"></param>
        /// <returns></returns>
        public static int GetRol(int Usuario_Id) {
            return _db.Usuario.Find(Usuario_Id).Rol_Id;
        }

        /// <summary>
        /// Obtiene el nombre de un usuario a partir de su ID
        /// </summary>
        /// <param name="Usuario_Id"></param>
        /// <returns></returns>
        internal static string GetNombre(int Usuario_Id) {
            return _db.Usuario.Find(Usuario_Id).nombre;            
        }       
                
        /// <summary>
        /// Comprueba si un usuario determinado posee el permiso indicado
        /// </summary>
        /// <param name="Usuario_id"></param>
        /// <param name="permiso"></param>
        /// <returns></returns>
        public static bool comprobarPermiso(int Usuario_id, TipoPermiso permiso) {
            int rol = GetRol(Usuario_id);
            return _db.Permiso.Any(p => p.Id == (int)permiso && p.Rol.Any(r => r.Id == rol));
        }

        /// <summary>
        /// Obtiene el idioma configurado por el usuario
        /// </summary>
        /// <param name="Usuario_Id"></param>
        /// <returns></returns>
        internal static object GetIdioma(int Usuario_Id) {
            return _db.Usuario.Find(Usuario_Id).idioma;
        }

        public static bool EsDocumentoEmpresaUsuario(int did)
        {
            int uid = GetUsuarioID();
            Usuario user = _db.Usuario.Find(uid);
            return _db.Documento.Any(x => x.Id == did && x.Propietario.Any(p => p.Empresa_Id == user.Empresa_Id));
        }
        public static bool EsTrabajadorEmpresaUsuario(int tid)
        {
            int uid = GetUsuarioID();
            Usuario user = _db.Usuario.Find(uid);
            return _db.Trabajador.Any(x => x.Id == tid && x.Empresa_Id == user.Empresa_Id);
        }
        public static bool EsEmpresaUsuario(int eid)
        {
            int uid = GetUsuarioID();
            Usuario user = _db.Usuario.Find(uid);
            return user.Empresa_Id == eid;
        }
    }
}