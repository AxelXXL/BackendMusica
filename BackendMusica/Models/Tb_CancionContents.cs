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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Tb_CancionContents
    {
        public long ID_CancionContents { get; set; }
        public long ID_Cancion { get; set; }
        public byte[] File_Content { get; set; }

        [JsonIgnore]
        public virtual Tb_Cancion Tb_Cancion { get; set; }
    }
}