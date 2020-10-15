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
    public class BaseDatosDA: BaseDA
    {
        const string Nombre_Clase = "BaseDatosDA";

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
        public bool Insertar(BaseDatosEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("spActualizacion_Base_Datos_BaseDatos_Servidor_Insert", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iBaseDatosId", obj.baseDatosId);
                    comandoSql.Parameters.AddWithValue("p_iServidorId", obj.servidorId);
                    comandoSql.Parameters.AddWithValue("p_iAccionId", obj.accionId);
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
        public bool Actualizar(BaseDatosEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_BaseDatos_Servidor_Update", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iBaseDatosId", obj.baseDatosId);
                    comandoSql.Parameters.AddWithValue("p_iServidorId", obj.servidorId);
                    comandoSql.Parameters.AddWithValue("p_iAccionId", obj.accionId);
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
        public bool Eliminar(BaseDatosEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Delete", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iBaseDatosId", obj.baseDatosId);

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
        public BaseDatosEN Seleccionar(int id)
        {
            BaseDatosEN o_Area = null;
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
                            o_Area = new BaseDatosEN(lectorSql, 0);
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
        public List<BaseDatosEN> Listar()
        {
            List<BaseDatosEN> lista = new List<BaseDatosEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_BaseDatos_SelectAll", conexionSql);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new BaseDatosEN(lectorSql, 0));
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
        public BaseDatosEN ListarServidores(int id)
        {
            BaseDatosEN obj = null;
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_BaseDatos_Servidor_Select_BaseDatosId", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_iBaseDatosId", id);

                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            obj = new BaseDatosEN(lectorSql, 1);
                        }
                    }
                    return obj;
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
        public List<BaseDatosEN> ListarPorRespaldo(int respaldoId)
        {
            List<BaseDatosEN> lista = new List<BaseDatosEN>();
            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_BaseDatos_Select_Respaldo", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_bitRespaldo", respaldoId);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new BaseDatosEN(lectorSql, 2));
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
        
        public List<BaseDatosEN> ListarPorCriterios(string criterio)
        {
            List<BaseDatosEN> lista = new List<BaseDatosEN>();
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
                            lista.Add(new BaseDatosEN(lectorSql, 0));
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

        
    }
}
