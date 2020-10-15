using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class BaseDatosEN : BaseEN
    {
        public readonly string Table_Name = "BaseDatosEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int baseDatosId { get; set; }
        [Required(ErrorMessage = "Código requerido.")]
        public int servidorId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string nombre { get; set; }
        public int accionId { get; set; }

        public List<ServidorEN> servidores { get; set; }


        public BaseDatosEN() { }
        public BaseDatosEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        baseDatosId = ValidarInt(Registro["c_iBaseDatosId"]);
                        servidorId = ValidarInt(Registro["c_iServidorId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        accionId = ValidarInt(Registro["c_iAccionId"]);
                        usuarioCreacion = ValidarString(Registro["c_vUsuarioRegistro"]);
                        fechaCreacion = Convert.ToDateTime(Registro["c_dtFechaRegistro"]);
                        usuarioActualizacion = ValidarString(Registro["c_vUsuarioModificacion"]);
                        fechaActualizacion = Convert.ToDateTime(Registro["c_dtFechaModificacion"]);
                        estado = ValidarString(Registro["c_cEstado"]);
                        break;
                    case 1:
                        //baseDatosId = ValidarInt(Registro["c_iBaseDatosId"]);
                        //servidorId = ValidarInt(Registro["c_iServidorId"]);
                        //nombre = ValidarString(Registro["c_vNombre"]);
                        //accionId = ValidarInt(Registro["c_iAccionId"]);
                        //usuarioCreacion = ValidarString(Registro["c_vUsuarioRegistro"]);
                        //fechaCreacion = Convert.ToDateTime(Registro["c_dtFechaRegistro"]);
                        //usuarioActualizacion = ValidarString(Registro["c_vUsuarioModificacion"]);
                        //fechaActualizacion = Convert.ToDateTime(Registro["c_dtFechaModificacion"]);
                        //estado = ValidarString(Registro["c_cEstado"]);
                        ServidorEN servidor = new ServidorEN()
                        {
                            servidorId = ValidarInt(Registro["c_iServidorId"]),
                            nombre = ValidarString(Registro["c_vNombre"]),
                            respaldoId = ValidarInt(Registro["c_bitRespaldo"]),
                            copiaId = ValidarInt(Registro["c_bitCopiar"]),
                            restauracionId = ValidarInt(Registro["c_bitRestaurar"])
                        };

                        servidores.Add(servidor);
                        break;
                    case 2:
                        baseDatosId = ValidarInt(Registro["c_iBaseDatosId"]);
                        nombre = ValidarString(Registro["c_vNombre"]);
                        accionId = ValidarInt(Registro["c_iAccionId"]);
                      
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
