using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendMusica.Models
{
    public class ExtendModel
    {
    }

    public partial class Tb_Usuario
    {
        public string ConfirmarContrasena { get; set; }
    }

    public class CancionData
    {
        public long ID_Cancion { get; set; }
        public string Nombre_Cancion { get; set; }
        public decimal? Numero_Cancion { get; set; }
        public int Duracion_Cancion { get; set; }
        public long ID_Artista { get; set; }
        public string Nombre_Artista { get; set; }
        public long ID_Album { get; set; }
        public string Nombre_Album { get; set; }
        public string Ruta_Audio { get; set; }
        public byte[] Caratula_Cancion { get; set; }
        public byte[] File_Content { get; set; } // Propiedad para almacenar el contenido del archivo
    }

    public class AlbumInfo
    {
        public long ID_Album { get; set; }
        public long ID_Artista { get; set; }
        public string Nombre_Album { get; set; }
        public string Nombre_Artista { get; set; }
        public string Genero { get; set; }
        public Nullable<int> Año_Album { get; set; }
        public byte[] Caratula_Album { get; set; }
    }
    

    public class RolModel
    {
        public int ID_Rol { get; set; }
        public string Rol { get; set; }
        public string Descripcion { get; set; }
    }

}