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
    public class ReporteProgramadoPendienteDA: BaseDA
    {
        const string Nombre_Clase = "ReporteProgramadoPendienteDA";

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
        public bool Insertar(ReporteProgramadoPendienteEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spReporteProgradolog_IngresarPendiente", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iIdUsuario", obj.usuarioId);
                    comandoSql.Parameters.AddWithValue("p_vTipoReporte", obj.tipoReporte);
                    comandoSql.Parameters.AddWithValue("p_iSecuencia", obj.secuencia);
                    comandoSql.Parameters.AddWithValue("p_vFechaInicial", obj.fechaInicial);
                    comandoSql.Parameters.AddWithValue("p_vFechaFinal", obj.fechaFinal);
                    comandoSql.Parameters.AddWithValue("p_vErrorEnvio", obj.errorEnvio);
                    comandoSql.Parameters.AddWithValue("p_vParametros", obj.parametros);

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
        public bool Actualizar(ReporteProgramadoPendienteEN obj)
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
        public bool Eliminar(ReporteProgramadoPendienteEN obj)
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
        public ReporteProgramadoPendienteEN Seleccionar(int id)
        {
            ReporteProgramadoPendienteEN o_Area = null;
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
                            o_Area = new ReporteProgramadoPendienteEN(lectorSql, 0);
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
        public List<ReporteProgramadoPendienteEN> Listar(ReporteProgramadoPendienteEN obj)
        {
            List<ReporteProgramadoPendienteEN> lista = new List<ReporteProgramadoPendienteEN>();
            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.sp_AplicativoHades_ListarPorCriterio", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;
                    comandoSql.Parameters.AddWithValue("p_iIdUsuario", obj.usuarioId);
                    conexionSql.Open();
                    using (SqlDataReader lectorSql = comandoSql.ExecuteReader())
                    {
                        while (lectorSql.Read())
                        {
                            lista.Add(new ReporteProgramadoPendienteEN(lectorSql, 0));
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
        public List<ReporteProgramadoPendienteEN> ListarPorCriterios(string criterio)
        {
            List<ReporteProgramadoPendienteEN> lista = new List<ReporteProgramadoPendienteEN>();
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
                            lista.Add(new ReporteProgramadoPendienteEN(lectorSql, 0));
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

        public ReporteProgramadoPendienteEN ObtenerRespuesta(ReporteProgramadoPendienteEN obj)
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

        public bool ActualizarForzarFin(ReporteProgramadoPendienteEN obj)
        {

            using (SqlConnection conexionSql = Conectar2(obj.baseDatosId))
            {
                try
                {
                    SqlCommand comandoSql = new SqlCommand("dbo.spReporteProgramadoPendienteForzarFin", conexionSql);
                    comandoSql.CommandType = CommandType.StoredProcedure;

                    comandoSql.Parameters.AddWithValue("p_iIdUsuario", obj.usuarioId);
                    comandoSql.Parameters.AddWithValue("p_vTipoReporte", obj.tipoReporte);
                    comandoSql.Parameters.AddWithValue("p_iSecuencia", obj.secuencia);
                    comandoSql.Parameters.AddWithValue("p_vIntervalo", obj.intervalo);
                    comandoSql.Parameters.AddWithValue("p_vParametros", obj.parametros);
                    
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

        
    }
}
