using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class ActivoEN : BaseEN
    {
        public readonly string Table_Name = "ActivoEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int activoId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string alias { get; set; }
        [Required(ErrorMessage = "Ejecutable requerido.")]
        public string vid { get; set; }
        public int usuarioId { get; set; }
        public string entidadId { get; set; }
        public string chasis { get; set; }

        public ActivoEN() { }
        public ActivoEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        activoId = ValidarInt(Registro["IdActivo"]);
                        alias = ValidarString(Registro["Alias"]);
                        vid = ValidarString(Registro["VID"]);
                        chasis = ValidarString(Registro["Chasis"]);
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
