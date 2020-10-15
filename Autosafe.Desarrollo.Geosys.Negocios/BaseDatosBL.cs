using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class BaseDatosBL
    {
        public int ObtenerId()
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.ObtenerId();
        }
        public bool Insertar(BaseDatosEN obj)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.Insertar(obj);
        }
        public bool Actualizar(BaseDatosEN obj)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.Actualizar(obj);
        }
        public bool Eliminar(BaseDatosEN obj)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.Eliminar(obj);
        }
        public BaseDatosEN Seleccionar(int areaId)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.Seleccionar(areaId);
        }
        public List<BaseDatosEN> Listar()
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.Listar();
        }
        public List<ServidorEN> ListarServidores(int id)
        {
            BaseDatosDA datos = new BaseDatosDA();
            BaseDatosEN obj = datos.ListarServidores(id);

            return obj.servidores;
        }
        public List<BaseDatosEN> ListarPorRespaldo(int respaldoId)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.ListarPorRespaldo(respaldoId);
        }
        public List<BaseDatosEN> ListarPorCriterios(string criterio)
        {
            BaseDatosDA datos = new BaseDatosDA();
            return datos.ListarPorCriterios(criterio);
        }
        
    }
}
