using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ActivoBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(TipoServidorEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.Insertar(obj);
        }
        public bool Actualizar(TipoServidorEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.Actualizar(obj);
        }
        public bool Eliminar(TipoServidorEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.Eliminar(obj);
        }
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<TipoServidorEN> Listar()
        {
            ActivoDA datos = new ActivoDA();
            return datos.Listar();
        }
        public List<MonitoreoHadesEN> ListarPorCriterios(string criterio)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ListarPorCriterios(criterio);
        }
        public List<ActivoEN> ListarActivosSinReportar(ActivoEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.ListarActivosSinReportar(obj);
        }
        public int ListarVehiculosActivos(ActivoEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.ListarVehiculosActivos(obj);
        }
        public ActivoEN GetVidPorPlaca(ActivoEN obj)
        {
            ActivoDA datos = new ActivoDA();
            return datos.GetVidPorPlaca(obj);
        }
    }
}
