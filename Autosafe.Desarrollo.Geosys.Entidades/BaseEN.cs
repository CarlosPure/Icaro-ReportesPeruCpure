using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class BaseEN
    {

        public BaseEN () {
            this.baseDatosId = 0;
        }

        [ScriptIgnore]
        public CultureInfo culture = new CultureInfo("es-PE");
        #region Propiedades de Auditoria
        [DataMember]
        public bool flagAnulado { get; set; }
        [DataMember]
        public string usuarioCreacion { get; set; }
        [DataMember]
        public DateTime? fechaCreacion { get; set; }
        [DataMember]
        public string usuarioActualizacion { get; set; }
        [DataMember]
        public DateTime? fechaActualizacion { get; set; }
        [DataMember]
        public string idFuncion { get; set; }
        [DataMember]
        public string nroIp { get; set; }
        [DataMember]
        public string nroMac { get; set; }
        [DataMember]
        public string usuarioBaseDatos { get; set; }
        [DataMember]
        public int estadoId { get; set; }
        [DataMember]
        public string estado { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public DateTime? fechaEstado { get; set; }
        [DataMember]
        public int baseDatosId { get; set; }
        #endregion

        #region Validación de Booleanos
        public bool ValidarBool(Object valor)
        {
            bool vRespuesta = false;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (bool)valor; }
            return vRespuesta;
        }
        #endregion
        #region Validación de Cadenas
        public string ValidarString(Object valor)
        {
            string vRespuesta = null;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (string)Convert.ToString(valor).Trim(); }
            return vRespuesta;
        }
        #endregion
        #region Validación de Enteros
        public int ValidarInt(Object valor)
        {
            int vRespuesta = 0;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (int)valor; }
            return vRespuesta;
        }
        #endregion
        #region Validación de Decimales
        public double ValidarDouble(Object valor)
        {
            double vRespuesta = 0;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = Convert.ToDouble(valor, culture); }
            return vRespuesta;
        }
        public decimal ValidarDecimal(Object valor)
        {
            decimal vRespuesta = 0;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = Convert.ToDecimal(valor, culture); }
            return vRespuesta;
        }
        #endregion
        #region Validación de Fechas
        public DateTime? ValidarDate(Object valor)
        {
            DateTime? vRespuesta = null;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (DateTime)valor; }
            return vRespuesta;
        }
        protected internal DateTime ValidarDatetime(Object valor)
        {
            DateTime vRespuesta = Convert.ToDateTime("01/01/0001");
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (DateTime)valor; }
            return vRespuesta;
        }
        #endregion
        #region Validacion de Arreglos
        protected internal byte[] ValidarByte(Object valor)
        {
            byte[] vRespuesta = null;
            if (!DBNull.Value.Equals(valor)) { vRespuesta = (byte[])valor; }
            return vRespuesta;
        }
        #endregion
    }
}
