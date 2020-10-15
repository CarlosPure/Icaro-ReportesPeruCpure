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
    public class ReporteProgramadoLogDA: BaseDA
    {
        const string Nombre_Clase = "ReporteProgramadoLogDA";

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
        public bool Insertar(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spReporteProgradolog_Ingresar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iIdUsuario", obj.usuarioId);
                    comandoSql.Parameters.AddWithValue("p_vTipoReporte", obj.tipoReporte);
                    comandoSql.Parameters.AddWithValue("p_vObservaciones", obj.observaciones);
                    comandoSql.Parameters.AddWithValue("p_vEmail", obj.email);

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
        public bool Actualizar(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Update", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iTipoServidorId", obj.usuarioId);
                    comandoSql.Parameters.AddWithValue("p_vNombre", obj.nombre);
                    comandoSql.Parameters.AddWithValue("p_vDescripcion", obj.email);
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
        public bool Eliminar(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spActualizacion_Base_Datos_TipoServidor_Delete", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iTipoServidorId", obj.usuarioId);

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
        public ReporteProgramadoLogEN Seleccionar(int id)
        {
            ReporteProgramadoLogEN o_Area = null;
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
                            o_Area = new ReporteProgramadoLogEN(lectorSql, 0);
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
        public List<ReporteProgramadoLogEN> Listar(ReporteProgramadoLogEN obj)
        {
            List<ReporteProgramadoLogEN> lista = new List<ReporteProgramadoLogEN>();
            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spReporteProgramadoListar", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_iIdUsuario", obj.usuarioId);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new ReporteProgramadoLogEN(lectorSql, 0));
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
        public List<ReporteProgramadoLogEN> ListarPorCriterios(string criterio)
        {
            List<ReporteProgramadoLogEN> lista = new List<ReporteProgramadoLogEN>();
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
                            lista.Add(new ReporteProgramadoLogEN(lectorSql, 0));
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

        public ReporteProgramadoLogEN ObtenerRespuesta(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar())
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand(obj.email, conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    conexionSql.Open();

                    obj.usuarioId = Convert.ToInt32(comandoSql.ExecuteScalar().ToString());

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

        public bool ActualizarForzarFin(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spReporteProgramadoForzarFin", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("Secuencia", obj.secuencia);
                    
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

        public ReporteProgramadoLogEN GetUltimoId(ReporteProgramadoLogEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("SELECT dbo.BuscarUltimoResumen(@idUsuario)", conexionSql);
                    comandoSql.CommandType = CommandType.Text;
                    comandoSql.Parameters.AddWithValue("@idUsuario", obj.usuarioId);

                    conexionSql.Open();

                    obj.secuencia = Int32.Parse(comandoSql.ExecuteScalar().ToString());

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
