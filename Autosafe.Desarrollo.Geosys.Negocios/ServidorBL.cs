using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ServidorBL
    {
        public int ObtenerId()
        {
            ServidorDA datos = new ServidorDA();
            return datos.ObtenerId();
        }
        public bool Insertar(ServidorEN obj)
        {
            ServidorDA datos = new ServidorDA();
            return datos.Insertar(obj);
        }
        public bool Actualizar(ServidorEN obj)
        {
            ServidorDA datos = new ServidorDA();
            return datos.Actualizar(obj);
        }
        public bool Eliminar(ServidorEN obj)
        {
            ServidorDA datos = new ServidorDA();
            return datos.Actualizar(obj);
        }
        public ServidorEN Seleccionar(int areaId)
        {
            ServidorDA datos = new ServidorDA();
            return datos.Seleccionar(areaId);
        }
        public List<ServidorEN> Listar()
        {
            ServidorDA datos = new ServidorDA();
            return datos.Listar();
        }
        public List<ServidorEN> ListarPorRespaldo(int respaldoId)
        {
            ServidorDA datos = new ServidorDA();
            return datos.ListarPorRespaldo(respaldoId);
        }
        public List<ServidorEN> ListarPorCriterios(string criterio)
        {
            ServidorDA datos = new ServidorDA();
            return datos.ListarPorCriterios(criterio);
        }
        
        public List<ServidorEN> ListarPorTipoServidor(int id)
        {
            ServidorDA datos = new ServidorDA();
            return datos.ListarPorTipoServidor(id);
        }
        public ServidorEN SeleccionarPorcentaje(ServidorEN obj)
        {
            ServidorDA datos = new ServidorDA();
            return datos.SeleccionarPorcentaje(obj);
        }
    }
}
