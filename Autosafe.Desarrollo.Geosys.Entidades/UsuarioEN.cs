using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class UsuarioEN : BaseEN
    {
        public readonly string Table_Name = "UsuarioEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int usuarioId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string usuario { get; set; }


        public UsuarioEN() { }
        public UsuarioEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        usuarioId = ValidarInt(Registro["IdUsuario"]);
                        usuario = ValidarString(Registro["Usuario"]);
                        break;

                }
                  
            }
            catch (Exception ex)
            {
                throw new Exception("Clase Entidades " + Table_Name + "\r\n" + "Descripción: " + ex.Message);
            }
        }
    }
}
