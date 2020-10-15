using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class UsuarioBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        public bool Insertar(TipoServidorEN obj)
        {
            UsuarioDA datos = new UsuarioDA();
            return datos.Insertar(obj);
        }
        public bool Actualizar(TipoServidorEN obj)
        {
            UsuarioDA datos = new UsuarioDA();
            return datos.Actualizar(obj);
        }
        public bool Eliminar(TipoServidorEN obj)
        {
            UsuarioDA datos = new UsuarioDA();
            return datos.Eliminar(obj);
        }
        public List<TipoServidorEN> Listar()
        {
            UsuarioDA datos = new UsuarioDA();
            return datos.Listar();
        }
        public List<MonitoreoHadesEN> ListarPorCriterios(string criterio)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ListarPorCriterios(criterio);
        }
        public UsuarioEN GetUsuarioPorId(UsuarioEN obj)
        {
            UsuarioDA datos = new UsuarioDA();
            return datos.GetUsuarioPorId(obj);
        }
    }
}
