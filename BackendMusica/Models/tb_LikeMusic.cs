//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BackendMusica.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tb_LikeMusic
    {
        public long ID_Likes { get; set; }
        public int ID_TypesLikes { get; set; }
        public int ID_Usuario { get; set; }
        public long ID_Cancion { get; set; }
    
        public virtual Tb_Cancion Tb_Cancion { get; set; }
        public virtual Tb_TypesLikes Tb_TypesLikes { get; set; }
        public virtual Tb_Usuario Tb_Usuario { get; set; }
    }
}
