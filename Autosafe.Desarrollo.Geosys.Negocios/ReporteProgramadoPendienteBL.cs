using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ReporteProgramadoPendienteBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(ReporteProgramadoPendienteEN obj)
        {
            ReporteProgramadoPendienteDA datos = new ReporteProgramadoPendienteDA();
            return datos.Insertar(obj);
        }
        //public bool Actualizar(ReporteProgramadoPendienteEN obj)
        //{
        //    ReporteProgramadoPendienteDA datos = new ReporteProgramadoPendienteDA();
        //    return datos.Actualizar(obj);
        //}
        //public bool Eliminar(ReporteProgramadoPendienteEN obj)
        //{
        //    ReporteProgramadoPendienteDA datos = new ReporteProgramadoPendienteDA();
        //    return datos.Eliminar(obj);
        //}
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<ReporteProgramadoPendienteEN> Listar(ReporteProgramadoPendienteEN obj)
        {
            ReporteProgramadoPendienteDA datos = new ReporteProgramadoPendienteDA();
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

        public bool ActualizarForzarFin(ReporteProgramadoPendienteEN obj)
        {
            ReporteProgramadoPendienteDA datos = new ReporteProgramadoPendienteDA();
            return datos.ActualizarForzarFin(obj);
        }
    }
}
