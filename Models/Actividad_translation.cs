
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
    
public partial class Actividad_translation
{

    public int Id { get; set; }

    public int Actividad_Id { get; set; }

    public string Idioma_codigo { get; set; }

    public string Descripcion { get; set; }



    public virtual Actividad Actividad { get; set; }

}

}
