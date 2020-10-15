using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class TipoServidorEN : BaseEN
    {
        public readonly string Table_Name = "TipoServidorEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int tipoServidorId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Ejecutable requerido.")]
        public string descripcion { get; set; }

        public TipoServidorEN() { }
        public TipoServidorEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        tipoServidorId = ValidarInt(Registro["c_iTipoServidorId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        descripcion = ValidarString(Registro["c_vDescripcion"]);
                        usuarioCreacion = ValidarString(Registro["c_vUsuarioRegistro"]);
                        fechaCreacion = Convert.ToDateTime(Registro["c_dtFechaRegistro"]);
                        usuarioActualizacion = ValidarString(Registro["c_vUsuarioModificacion"]);
                        fechaActualizacion = Convert.ToDateTime(Registro["c_dtFechaModificacion"]);
                        estado = ValidarString(Registro["c_cEstado"]);
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
