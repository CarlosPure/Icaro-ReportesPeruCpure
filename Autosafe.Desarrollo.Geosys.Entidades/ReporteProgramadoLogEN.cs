using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class ReporteProgramadoLogEN : BaseEN
    {
        public readonly string Table_Name = "ReporteProgramadoLogEN";

        [Required(ErrorMessage = "Código requerido.")]
        public int usuarioId { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Hora requerido.")]
        public string horaEnvio { get; set; }
        public string tipoReporte { get; set; }
        public string intervalo { get; set; }
        public string parametros { get; set; }
        public string activos { get; set; }
        public int secuencia { get; set; }
        public int horas { get; set; }
        public bool guardar { get; set; }
        public bool forzar { get; set; }
        public string nombre { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public string idEntidad { get; set; }

        public string observaciones { get; set; }
        public string errorEnvio { get; set; }

        public ReporteProgramadoLogEN() { }
        public ReporteProgramadoLogEN(IDataReader Registro, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        usuarioId = ValidarInt(Registro["IdUsuario"]);
                        email = ValidarString(Registro["Email"]);
                        horaEnvio = ValidarString(Registro["HoraEnvio"]);
                        tipoReporte = ValidarString(Registro["TipoReporte"]);
                        intervalo = ValidarString(Registro["Intervalo"]);
                        parametros = ValidarString(Registro["Parametros"]);
                        activos = ValidarString(Registro["Activos"]);
                        secuencia = ValidarInt(Registro["Secuencia"]);
                        horas = ValidarInt(Registro["Horas"]);
                        guardar = ValidarBool(Registro["Guardar"]);
                        forzar = ValidarBool(Registro["Forzar"]);
                        nombre = ValidarString(Registro["Nombre"]);
                        horaInicio = ValidarString(Registro["HoraInicio"]);
                        horaFin = ValidarString(Registro["HoraFin"]);
                        idEntidad = ValidarString(Registro["IdEntidad"]);
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
