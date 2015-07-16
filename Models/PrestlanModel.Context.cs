﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Prestlan.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Linq;


public partial class PRESTLANEntities : DbContext
{
    public PRESTLANEntities()
        : base("name=PRESTLANEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Actividad> Actividad { get; set; }

    public virtual DbSet<Actividad_translation> Actividad_translation { get; set; }

    public virtual DbSet<Biblioteca> Biblioteca { get; set; }

    public virtual DbSet<Empresa> Empresa { get; set; }

    public virtual DbSet<Fichero> Fichero { get; set; }

    public virtual DbSet<Idiomas> Idiomas { get; set; }

    public virtual DbSet<Log> Log { get; set; }

    public virtual DbSet<MovimientosDeDocumento> MovimientosDeDocumento { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Propietario> Propietario { get; set; }

    public virtual DbSet<Reclamaciones> Reclamaciones { get; set; }

    public virtual DbSet<Requisitos> Requisitos { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }

    public virtual DbSet<TipoDocumento_translation> TipoDocumento_translation { get; set; }

    public virtual DbSet<TipoEmpresa> TipoEmpresa { get; set; }

    public virtual DbSet<TipoEmpresa_translation> TipoEmpresa_translation { get; set; }

    public virtual DbSet<TipoEstado> TipoEstado { get; set; }

    public virtual DbSet<TipoEtapa> TipoEtapa { get; set; }

    public virtual DbSet<TipoIdentificador> TipoIdentificador { get; set; }

    public virtual DbSet<Tipoidentificador_translation> Tipoidentificador_translation { get; set; }

    public virtual DbSet<TipoTrabajador> TipoTrabajador { get; set; }

    public virtual DbSet<TipoTrabajador_translation> TipoTrabajador_translation { get; set; }

    public virtual DbSet<Trabajador> Trabajador { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Documento> Documento { get; set; }

    public virtual DbSet<Permiso> Permiso { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Configuracion> Configuracion { get; set; }


    public virtual ObjectResult<SELECT_DocumentosGenerarZIP_Result> SELECT_DocumentosGenerarZIP(string listaEmpresas, string listaTrabajadores, string listaRequisitos)
    {

        var listaEmpresasParameter = listaEmpresas != null ?
            new ObjectParameter("listaEmpresas", listaEmpresas) :
            new ObjectParameter("listaEmpresas", typeof(string));


        var listaTrabajadoresParameter = listaTrabajadores != null ?
            new ObjectParameter("listaTrabajadores", listaTrabajadores) :
            new ObjectParameter("listaTrabajadores", typeof(string));


        var listaRequisitosParameter = listaRequisitos != null ?
            new ObjectParameter("listaRequisitos", listaRequisitos) :
            new ObjectParameter("listaRequisitos", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SELECT_DocumentosGenerarZIP_Result>("SELECT_DocumentosGenerarZIP", listaEmpresasParameter, listaTrabajadoresParameter, listaRequisitosParameter);
    }


    public virtual ObjectResult<SELECT_DocumentosGenerarZIP_InfoEmpresa_Result> SELECT_DocumentosGenerarZIP_InfoEmpresa(string listaEmpresas, string listaRequisitos)
    {

        var listaEmpresasParameter = listaEmpresas != null ?
            new ObjectParameter("listaEmpresas", listaEmpresas) :
            new ObjectParameter("listaEmpresas", typeof(string));


        var listaRequisitosParameter = listaRequisitos != null ?
            new ObjectParameter("listaRequisitos", listaRequisitos) :
            new ObjectParameter("listaRequisitos", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SELECT_DocumentosGenerarZIP_InfoEmpresa_Result>("SELECT_DocumentosGenerarZIP_InfoEmpresa", listaEmpresasParameter, listaRequisitosParameter);
    }


    public virtual ObjectResult<SELECT_DocumentosGenerarZIP_InfoTrabajadores_Result> SELECT_DocumentosGenerarZIP_InfoTrabajadores(string listaTrabajadores, string listaRequisitos)
    {

        var listaTrabajadoresParameter = listaTrabajadores != null ?
            new ObjectParameter("listaTrabajadores", listaTrabajadores) :
            new ObjectParameter("listaTrabajadores", typeof(string));


        var listaRequisitosParameter = listaRequisitos != null ?
            new ObjectParameter("listaRequisitos", listaRequisitos) :
            new ObjectParameter("listaRequisitos", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SELECT_DocumentosGenerarZIP_InfoTrabajadores_Result>("SELECT_DocumentosGenerarZIP_InfoTrabajadores", listaTrabajadoresParameter, listaRequisitosParameter);
    }

}

}
