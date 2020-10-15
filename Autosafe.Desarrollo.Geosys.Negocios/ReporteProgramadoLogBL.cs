using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ReporteProgramadoLogBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(ReporteProgramadoLogEN obj)
        {
            ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
            return datos.Insertar(obj);
        }
        //public bool Actualizar(ReporteProgramadoLogEN obj)
        //{
        //    ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
        //    return datos.Actualizar(obj);
        //}
        //public bool Eliminar(ReporteProgramadoLogEN obj)
        //{
        //    ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
        //    return datos.Eliminar(obj);
        //}
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<ReporteProgramadoLogEN> Listar(ReporteProgramadoLogEN obj)
        {
            ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
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

        public bool ActualizarForzarFin(ReporteProgramadoLogEN obj)
        {
            ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
            return datos.ActualizarForzarFin(obj);
        }
        public ReporteProgramadoLogEN GetUltimoId(ReporteProgramadoLogEN obj)
        {
            ReporteProgramadoLogDA datos = new ReporteProgramadoLogDA();
            return datos.GetUltimoId(obj);
        }
    }
}
