using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class ServidorEN : BaseEN
    {
        public readonly string Table_Name = "ServidorEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int tipoServidorId { get; set; }
        [Required(ErrorMessage = "Código requerido.")]
        public int servidorId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "IP requerido.")]
        public string direccionIp { get; set; }
        public int puerto { get; set; }
        public string basedatos { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string rutaArchivo { get; set; }
        public int respaldoId { get; set; }
        public int copiaId { get; set; }
        public int restauracionId { get; set; }
        public string porcentaje { get; set; }

        public ServidorEN() { }
        public ServidorEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        tipoServidorId = ValidarInt(Registro["c_iServidorId"]);
                        tipoServidorId = ValidarInt(Registro["c_iTipoServidorId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        direccionIp = ValidarString(Registro["c_vIp"]);
                        puerto = ValidarInt(Registro["c_iPuerto"]);
                        basedatos = ValidarString(Registro["c_vNombreBaseDatos"]);
                        usuario = ValidarString(Registro["c_vUsuario"]);
                        clave = ValidarString(Registro["c_vClave"]);
                        rutaArchivo = ValidarString(Registro["c_vRuta"]);
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
