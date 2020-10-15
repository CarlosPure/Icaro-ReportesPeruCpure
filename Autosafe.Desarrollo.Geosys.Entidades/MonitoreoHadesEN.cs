using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class MonitoreoHadesEN : BaseEN
    {
        public readonly string Table_Name = "MonitoreoHadesEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int aplicativoHadesId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Ejecutable requerido.")]
        public string ejecutable { get; set; }
        public string ruta { get; set; }
        public string parametros { get; set; }
        public string direccionIp { get; set; }
        public string procedimientoAlmacenado { get; set; }
        public int respuestaSP { get; set; }
        public DateTime? ultimoReinicio { get; set; }
        public MonitoreoHadesEN() { }
        public MonitoreoHadesEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        aplicativoHadesId = ValidarInt(Registro["c_iAplicativoHadesId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        ejecutable = ValidarString(Registro["c_vEjecutable"]);
                        ruta = ValidarString(Registro["c_vRuta"]);
                        parametros = ValidarString(Registro["c_vParametros"]);
                        direccionIp = ValidarString(Registro["c_DireccionIp"]);
                        estado = ValidarString(Registro["c_cEstado"]);
                        procedimientoAlmacenado = ValidarString(Registro["c_vProcedimientoAlmacenado"]);
                        usuarioCreacion = ValidarString(Registro["c_vUsuarioRegistro"]);
                        fechaCreacion = Convert.ToDateTime(Registro["c_dtFechaRegistro"]);
                        usuarioActualizacion = ValidarString(Registro["c_vUsuarioModificacion"]);
                        fechaActualizacion = Convert.ToDateTime(Registro["c_dtFechaModificacion"]);
                        break;

                    case 1:
                        aplicativoHadesId = ValidarInt(Registro["c_iAplicativoHadesId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        ejecutable = ValidarString(Registro["c_vEjecutable"]);
                        ruta = ValidarString(Registro["c_vRuta"]);
                        parametros = ValidarString(Registro["c_vParametros"]);
                        direccionIp = ValidarString(Registro["c_DireccionIp"]);
                        procedimientoAlmacenado = ValidarString(Registro["c_vProcedimientoAlmacenado"]);
                        usuarioCreacion = ValidarString(Registro["c_vUsuarioRegistro"]);
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
