using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Entidades;

namespace Autosafe.Desarrollo.Geosys.Datos
{
    public class ActivoDA: BaseDA
    {
        const string Nombre_Clase = "ActivoDA";

        private int Id = 0;

        public int ObtenerId()
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    //TODO: POR VERIFICAR NOMBRE DE PROCEDIMIENTO ALMACENADO
                    SqlCommand comandoSql = new SqlCommand("peru.sp_TipoServidor_ObtenerId", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    conexionSql.Open();

                    Id = Convert.ToInt32(comandoSql.ExecuteScalar().ToString());

                    conexionSql.Close();

                    return Id;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }
        public bool Insertar(TipoServidorEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Insert", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_vNombre", obj.nombre);
                    comandoSql.Parameters.AddWithValue("p_vDescripcion", obj.descripcion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioRegistro", obj.usuarioCreacion);

                    conexionSql.Open();

                    comandoSql.ExecuteNonQuery();

                    conexionSql.Close();

                    return true;

                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }

        }
        public bool Actualizar(TipoServidorEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Update", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iTipoServidorId", obj.tipoServidorId);
                    comandoSql.Parameters.AddWithValue("p_vNombre", obj.nombre);
                    comandoSql.Parameters.AddWithValue("p_vDescripcion", obj.descripcion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioRegistro", obj.usuarioCreacion);
                    comandoSql.Parameters.AddWithValue("p_cEstado", obj.estado);

                    conexionSql.Open();

                    comandoSql.ExecuteNonQuery();

                    conexionSql.Close();

                    return true;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }
        public bool Eliminar(TipoServidorEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Delete", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iTipoServidorId", obj.tipoServidorId);

                    conexionSql.Open();

                    comandoSql.ExecuteNonQuery();

                    conexionSql.Close();

                    return true;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }
        public TipoServidorEN Seleccionar(int id)
        {
            TipoServidorEN o_Area = null;
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_Seleccionar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_iAplicativoHadesId", id);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        if (lectorSql.Read())
                        {
                            o_Area = new TipoServidorEN(lectorSql, 0);
                        }
                    }
                    conexionSql.Close();
                    return o_Area;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }
        public List<TipoServidorEN> Listar()
        {
            List<TipoServidorEN> lista = new List<TipoServidorEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_SelectAll", conexionSql);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new TipoServidorEN(lectorSql, 0));
                        }
                    }
                    return lista;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }
        public List<MonitoreoHadesEN> ListarPorCriterios(string criterio)
        {
            List<MonitoreoHadesEN> lista = new List<MonitoreoHadesEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_ListarPorCriterio", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("Criterio", criterio);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new MonitoreoHadesEN(lectorSql, 0));
                        }
                    }
                    conexionSql.Close();
                    return lista;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        public MonitoreoHadesEN ObtenerRespuesta(MonitoreoHadesEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand(obj.procedimientoAlmacenado, conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    conexionSql.Open();

                    obj.respuestaSP = Convert.ToInt32(comandoSql.ExecuteScalar().ToString());

                    conexionSql.Close();

                    return obj;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        public List<ServidorEN> ListarPorTipoServidor(int id)
        {
            List<ServidorEN> lista = new List<ServidorEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("spActualizacion_Base_Datos_Servidor_Select_TipoServidor", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_iTipoServidorId", id);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new ServidorEN(lectorSql, 0));
                        }
                    }
                    conexionSql.Close();
                    return lista;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        public List<ActivoEN> ListarActivosSinReportar(ActivoEN obj)
        {
            List<ActivoEN> lista = new List<ActivoEN>();
            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActivosSinReportar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("IdUsuario", obj.usuarioId);
                    comandoSql.CommandTimeout = 300;
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new ActivoEN(lectorSql, 0));
                        }
                    }
                    conexionSql.Close();
                    return lista;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        public int ListarVehiculosActivos(ActivoEN obj)
        {
            List<ActivoEN> lista = new List<ActivoEN>();
            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spCantidadVehiculosActivos", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("idUsuario", obj.usuarioId);
                    comandoSql.Parameters.AddWithValue("idEntidad", obj.entidadId);
                    comandoSql.CommandTimeout = 300;
                    conexionSql.Open();

                    object result = comandoSql.ExecuteScalar();
                    result = (result == DBNull.Value) ? null : result;

                    int cantidad = Convert.ToInt32(result);

                    conexionSql.Close();

                    return cantidad;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos: " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        public ActivoEN GetVidPorPlaca(ActivoEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("SELECT dbo.GetVIDPlaca(@Alias)", conexionSql);
                    comandoSql.CommandType = CommandType.Text;
                    comandoSql.Parameters.AddWithValue("@Alias", obj.alias);
                    comandoSql.CommandTimeout = 300;
                    conexionSql.Open();

                    obj.vid = comandoSql.ExecuteScalar().ToString();

                    conexionSql.Close();

                    return obj;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Clase Datos " + Nombre_Clase + "\r\n" + "Descripción: " + ex.Message);
                }
                finally
                {
                    conexionSql.Dispose();
                }
            }
        }

        
    }
}
