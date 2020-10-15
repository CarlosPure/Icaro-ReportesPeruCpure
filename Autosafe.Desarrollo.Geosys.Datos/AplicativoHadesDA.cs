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
    public class AplicativoHadesDA: BaseDA
    {
        const string Nombre_Clase = "AplicativoHadesDA";

        private int Id = 0;

        public int ObtenerId()
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_ObtenerId", conexionSql);
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
        public bool Insertar(MonitoreoHadesEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_Insertar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iAplicativoHadesId", obj.aplicativoHadesId);
                    comandoSql.Parameters.AddWithValue("p_vNombre", obj.nombre);
                    comandoSql.Parameters.AddWithValue("p_vEjecutable", obj.ejecutable);
                    comandoSql.Parameters.AddWithValue("p_vRuta", obj.ruta);
                    comandoSql.Parameters.AddWithValue("p_vParametros", obj.parametros);
                    comandoSql.Parameters.AddWithValue("p_DireccionIp", obj.direccionIp);
                    comandoSql.Parameters.AddWithValue("p_dtFechaRegistro", obj.fechaCreacion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioRegistro", obj.usuarioCreacion);
                    comandoSql.Parameters.AddWithValue("p_dtFechaModificacion", obj.fechaActualizacion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioModificacion", obj.usuarioActualizacion);
                    comandoSql.Parameters.AddWithValue("p_cEstado", obj.estado);
                    comandoSql.Parameters.AddWithValue("p_vProcedimientoAlmacenado", obj.procedimientoAlmacenado);

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
        public bool Actualizar(MonitoreoHadesEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_Actualizar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iAplicativoHadesId", obj.aplicativoHadesId);
                    comandoSql.Parameters.AddWithValue("p_vNombre", obj.nombre);
                    comandoSql.Parameters.AddWithValue("p_vEjecutable", obj.ejecutable);
                    comandoSql.Parameters.AddWithValue("p_vRuta", obj.ruta);
                    comandoSql.Parameters.AddWithValue("p_vParametros", obj.parametros);
                    comandoSql.Parameters.AddWithValue("p_DireccionIp", obj.direccionIp);
                    comandoSql.Parameters.AddWithValue("p_dtFechaRegistro", obj.fechaCreacion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioRegistro", obj.usuarioCreacion);
                    comandoSql.Parameters.AddWithValue("p_dtFechaModificacion", obj.fechaActualizacion);
                    comandoSql.Parameters.AddWithValue("p_vUsuarioModificacion", obj.usuarioActualizacion);
                    comandoSql.Parameters.AddWithValue("p_cEstado", obj.estado);
                    comandoSql.Parameters.AddWithValue("p_vProcedimientoAlmacenado", obj.procedimientoAlmacenado);
                    comandoSql.Parameters.AddWithValue("p_dtUltimoReinicio", obj.ultimoReinicio);

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
        public MonitoreoHadesEN Seleccionar(int id)
        {
            MonitoreoHadesEN o_Area = null;
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
                            o_Area = new MonitoreoHadesEN(lectorSql, 0);
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
        public List<MonitoreoHadesEN> Listar()
        {
            List<MonitoreoHadesEN> lista = new List<MonitoreoHadesEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("peru.sp_AplicativoHades_Listar", conexionSql);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new MonitoreoHadesEN(lectorSql, 0));
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

        
    }
}
