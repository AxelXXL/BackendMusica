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
    
    public partial class Tb_Artista
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tb_Artista()
        {
            this.Tb_Album = new HashSet<Tb_Album>();
            this.Tb_Cancion = new HashSet<Tb_Cancion>();
            this.Tb_Seguidos = new HashSet<Tb_Seguidos>();
        }
    
        public long ID_Artista { get; set; }
        public string Nombre_Artista { get; set; }
        public bool Activo { get; set; }
        public byte[] Imagen_Artista { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Album> Tb_Album { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Cancion> Tb_Cancion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Seguidos> Tb_Seguidos { get; set; }
    }
}
