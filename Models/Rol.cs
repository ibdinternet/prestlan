
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
    
public partial class Rol
{

    public Rol()
    {

        this.Usuario = new HashSet<Usuario>();

        this.Permiso = new HashSet<Permiso>();

    }


    public int Id { get; set; }

    public string descripcion { get; set; }



    public virtual ICollection<Usuario> Usuario { get; set; }

    public virtual ICollection<Permiso> Permiso { get; set; }

}

}
