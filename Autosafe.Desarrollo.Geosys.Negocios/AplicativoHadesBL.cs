using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class AplicativoHadesBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(MonitoreoHadesEN obj)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Insertar(obj);
        }
        public bool Actualizar(MonitoreoHadesEN obj)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Actualizar(obj);
        }
        public bool Eliminar(MonitoreoHadesEN obj)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Actualizar(obj);
        }
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<MonitoreoHadesEN> Listar()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Listar();
        }
        public List<MonitoreoHadesEN> ListarPorCriterios(string criterio)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ListarPorCriterios(criterio);
        }
        public MonitoreoHadesEN ObtenerRespuesta(MonitoreoHadesEN obj)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerRespuesta(obj);
        }
    }
}
