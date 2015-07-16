using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prestlan.Models
{

    [Serializable]
    public sealed class EstadoRetorno
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
    }

    public enum TiposDeError
    {
        // Documentos
        DOCUMENTO_NO_EXISTE = 10,
        DOCUMENTO_CADUCADO = 11,
        DOCUMENTO_SIN_FECHA_CADUCIDAD = 12,
        DOCUMENTO_NO_EN_REVISION = 13,
        DOCUMENTO_NO_ES_BORRADOR = 14,
        DOCUMENTO_NO_PUBLICADO = 15,
        DOCUMENTO_AUN_NO_CADUCADO = 16,
        DOCUMENTO_YA_VALIDADO = 17,
        DOCUMENTO_YA_CADUCADO = 18,
        // Usuarios
        USUARIO_NO_AUTORIZADO = 20
    }
    public enum TiposDeNotificaciones
    {
        PasoARevision,
        Rechazado,
        Reclamacion,
        Caducado
    }
    public enum TiposDeRoles
    {
        ADMIN = 1,
        VALIDADOR = 2,
        SUBCONTRATA = 3,
        FREE = 4
    }
    public enum TiposDeEtapa
    {
        BORRADOR = 1,
        REVISION = 2,
        RECHAZO = 3,
        PUBLICACION = 4,
        CADUCAR = 5
    }
    public enum TiposDeEstado
    {
        BORRADOR = 1,
        PUBLICADO = 2,
        CADUCADO = 3
    }
    public enum TipoPermiso
    {
        VerAccesoAdministracion = 1,
        VerAccesoDocumentos = 2,
        VerAccesoSubcontratas = 3,
        VerAccesoHomologaciones = 4,
        VerAccesoReclamaciones = 5,
        VerAccesoTrabajadores = 6,
        VerAccesoValidaciones = 7,
        VerAccesoEmpresa = 8,
        VerAccesoRequisitos = 9,
        VerAccesoGenerarZIP = 10,
        DocumentosListarTodo = 11,
        DocumentosListarSoloEmpresa = 12,
        DocumentosEditarTodo = 13,
        DocumentosEditarSoloEmpresa = 14,
        TrabajadoresListarTodo = 15,
        TrabajadoresListarSoloEmpresa = 16,
        TrabajadoresEditarTodo = 17,
        TrabajadoresEditarSoloEmpresa = 18,
        EmpresasListarTodo = 19,
        EmpresasListarSoloEmpresa = 20,
        EmpresasEditarTodo = 21,
        EmpresasEditarSoloEmpresa = 22,
        ReclamacionesListar = 23,
        ReclamacionesEditar = 24,
        ValidacionesListar = 25,
        ValidacionesEditar = 26,
        FicherosDescargarTodo = 27,
        FicherosDescargarSoloEmpresa = 28,
        RequisitosListar = 29,
        RequisitosEditar = 30,
        NotificacionesCaducidadListarTodo = 31,
        NotificacionesCaducidadListarSoloEmpresa = 32
    }
    public class ListNotificaciones
    {
        public int Documento_Id { get; set; }
        public string Descripcion { get; set; }

        public bool EstaCaducado { get; set; }
    }
    public class ListDocumentoPropietario
    {
        public int Propietario_Id { get; set; }
        public int Documento_Id { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string Nombre { get; set; }

        public int? Trabajador_Id { get; set; }

        public int? Empresa_Id { get; set; }

        public int TipoDocumento_Id { get; set; }

        public string Etapa { get; set; }
    }
    public class ListDocumentoPropietarioSearch
    {
        public int Propietario_Id { get; set; }
        public int Documento_Id { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string Nombre { get; set; }
        public int? Empresa_Id { get; set; }
        public int? Trabajador_Id { get; set; }
        public int? Actividad_Id { get; set; }
        public int TipoDocumento_Id { get; set; }
        public int Fichero_Id { get; set; }
        public int Estado_Id { get; set; }
    }
    public class ListDocumentoPropietarioValidacion
    {
        public int Id { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string Nombre { get; set; }
        public string Creador { get; set; }
        public string Revisor { get; set; }
        public string Archivo { get; set; }
        public int Fichero_id { get; set; }
        public int? Empresa_Id { get; set; }
        public int? Trabajador_Id { get; set; }
        public int TipoDocumento_Id { get; set; }
        public int? DocumentoVersion_Id { get; set; }
    }
    public class DocumentoFichero
    {
        public int Documento_Id { get; set; }
        public int Estado_Id { get; set; }
        public string Estado { get; set; }
        public int Etapa_Id { get; set; }
        public string Etapa { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public int Fichero_Id { get; set; }
    }

    public class ListDocumento
    {
        public string Id;
        public string Titulo;
        public string Autor;
        public string Descripcion;
        public string Estado;
        public string EstadoDescripcion;

        public int Estado_Id { get; set; }

        public int Etapa_Id { get; set; }

        public string EtapaDescripcion { get; set; }

        public string Etapa { get; set; }

        public bool PuedeValidar { get; set; }
    }
    public class ListDocumentos
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Caduca { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool SinPropietarios { get; set; }
        public bool SinNotificaciones { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime FechaActualizado { get; set; }
        public bool Eliminado { get; set; }
        public string HashEmpleado { get; set; }
        public int Fichero_Id { get; set; }
        public int? DocumentoVersion_Id { get; set; }
        public int TipoDocumento_Id { get; set; }
        public int Usuario_Id { get; set; }
        public int Estado_Id { get; set; }
        public int Etapa_Id { get; set; }
    }
    public class ListEmpresa
    {
        public int Id { get; set; }
        public string ValorIdentificador { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public bool SinNotificaciones { get; set; }
        public string CorreosNotificaciones { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
        public string TipoIdentificador { get; set; }
        public string TipoEmpresa { get; set; }
        public string Actividad { get; set; }
        public int Tipoempresa_Id { get; set; }
        public int Actividad_Id { get; set; }
    }
    public class ListTrabajador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Empresa { get; set; }
        public string Empleado { get; set; }
        public string ValorIdentificador { get; set; }
        public string Actividad { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Empresa_Id { get; set; }
    }
    public class ListTipoTrabajador
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
    public class ListActividades
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class ListTipoDocumento
    {
        public int Id { get; set; }
        public bool MultiplesPropietarios { get; set; }
        public int NivelCriticidad { get; set; }
        public string Idioma_codigo { get; set; }
        public string Descripcion { get; set; }
        public string TextoAyuda { get; set; }
        public string Plantilla { get; set; }

        public bool EsDocumentoDeEmpresa { get; set; }
    }
    public class ListUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public string Ip { get; set; }
        public int Rol_Id { get; set; }
        public string Rol { get; set; }
        public int? Empresa_Id { get; set; }
        public string Empresa { get; set; }
    }
    public class ListTipoIdentificador
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Idioma_codigo { get; set; }
    }
    public class ListPermisos
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int? Rol_Id { get; set; }
        public string Rol { get; set; }
    }

    public class ListDocumentoGeneracionZIP
    {
        public string NombreFS { get; set; }
        public int? Requisito_Id { get; set; }
        public int? Empresa_Id { get; set; }
        public int? Trabajador_Id { get; set; }
    }

    public class ListRequisitoGeneracionZIP
    {
        public int Requisito_Id { get; set; }
        public int TipoDocumento_Id { get; set; }
    }
}