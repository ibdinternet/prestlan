
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
    using System.Collections.Generic;
    
public partial class TipoDocumento
{

    public TipoDocumento()
    {

        this.TipoDocumento_translation = new HashSet<TipoDocumento_translation>();

        this.Requisitos = new HashSet<Requisitos>();

        this.Documento = new HashSet<Documento>();

    }


    public int Id { get; set; }

    public bool MultiplesPropietarios { get; set; }

    public int NivelCriticidad { get; set; }

    public bool EsDocumentoDeEmpresa { get; set; }



    public virtual ICollection<TipoDocumento_translation> TipoDocumento_translation { get; set; }

    public virtual ICollection<Requisitos> Requisitos { get; set; }

    public virtual ICollection<Documento> Documento { get; set; }

}

}
