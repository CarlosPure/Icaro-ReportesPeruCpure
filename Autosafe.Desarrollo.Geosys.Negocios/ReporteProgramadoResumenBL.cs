using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ReporteProgramadoResumenBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(ReporteProgramadoResumenEN obj)
        {
            ReporteProgramadoResumenDA datos = new ReporteProgramadoResumenDA();
            return datos.Insertar(obj);
        }
        //public bool Actualizar(ReporteProgramadoResumenEN obj)
        //{
        //    ReporteProgramadoResumenDA datos = new ReporteProgramadoResumenDA();
        //    return datos.Actualizar(obj);
        //}
        //public bool Eliminar(ReporteProgramadoResumenEN obj)
        //{
        //    ReporteProgramadoResumenDA datos = new ReporteProgramadoResumenDA();
        //    return datos.Eliminar(obj);
        //}
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<ReporteProgramadoResumenEN> Listar(ReporteProgramadoResumenEN obj)
        {
            ReporteProgramadoResumenDA datos = new ReporteProgramadoResumenDA();
            return datos.Listar(obj);
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

        public bool ActualizarForzarFin(ReporteProgramadoResumenEN obj)
        {
            ReporteProgramadoResumenDA datos = new ReporteProgramadoResumenDA();
            return datos.ActualizarForzarFin(obj);
        }
    }
}
