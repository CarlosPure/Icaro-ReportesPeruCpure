using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autosafe.Desarrollo.Geosys.Datos;
using Autosafe.Desarrollo.Geosys.Entidades;
using System.Net;
using System.IO;

namespace Autosafe.Desarrollo.Geosys.Negocios
{
    public class ReporteProgramadoBL
    {
        public int ObtenerId()
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.ObtenerId();
        }
        //public bool Insertar(ReporteProgramadoEN obj)
        //{
        //    ReporteProgramadoDA datos = new ReporteProgramadoDA();
        //    return datos.Insertar(obj);
        //}
        //public bool Actualizar(ReporteProgramadoEN obj)
        //{
        //    ReporteProgramadoDA datos = new ReporteProgramadoDA();
        //    return datos.Actualizar(obj);
        //}
        //public bool Eliminar(ReporteProgramadoEN obj)
        //{
        //    ReporteProgramadoDA datos = new ReporteProgramadoDA();
        //    return datos.Eliminar(obj);
        //}
        public MonitoreoHadesEN Seleccionar(int areaId)
        {
            AplicativoHadesDA datos = new AplicativoHadesDA();
            return datos.Seleccionar(areaId);
        }
        public List<ReporteProgramadoEN> Listar(ReporteProgramadoEN obj)
        {
            ReporteProgramadoDA datos = new ReporteProgramadoDA();
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

        public bool ActualizarForzarFin(ReporteProgramadoEN obj)
        {
            ReporteProgramadoDA datos = new ReporteProgramadoDA();
            return datos.ActualizarForzarFin(obj);
        }

        public String enviar(String[] correos, String nombre, String usuario, String clave, String url, String token, String evento, String asunto)
        {
            String AccessToken = token;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            //httpWebRequest.PreAuthenticate = true;
            //httpWebRequest.Headers.Add("Authorization", "" + AccessToken);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                StringBuilder sb = new StringBuilder();
                
            sb.Append("{");
                sb.Append("\"eventName\": \"" + evento + "\",");
                sb.Append("\"items\": [");

                int contador = 0;

                foreach (string correo in correos)
                {
                    sb.Append("{");
                        sb.Append("\"email\": \"" + correo + "\",");
                        sb.Append("\"attributes\": {");
    	                    sb.Append("\"nombre\":\"" + nombre + "\",");
			                sb.Append("\"usuario\":\"" + usuario + "\",");
			                sb.Append("\"clave\":\"" + clave + "\",");
			                sb.Append("\"asunto\":\"" + asunto + "\",");
                            sb.Append("\"event_items\": [");
                            sb.Append("]");
                        sb.Append("}");
                    sb.Append("}");
                    
                    //sumar contador
                    contador++;

                    if ( contador < correos.Length) {
                        sb.Append(",");
                    }
                }

                sb.Append("]");
                sb.Append("}");

                streamWriter.Write(sb.ToString());
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                
                return result.ToString();
            }   
        }


        public String enviarunomensual(String[] correos, String url, String token, String evento, ResumenEjecutivoUnoEN objResumen)
        {
            String AccessToken = token;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Headers.Add("Authorization", "" + AccessToken);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                
                StringBuilder sb = new StringBuilder();
             
                sb.Append("{");
                sb.Append("\"eventName\": \"" + evento + "\",");
                sb.Append("\"items\": [");

                int contador = 0;

                foreach (string correo in correos)
                {
                    string email = correo.Replace("$", "");

                    sb.Append("{");
                    sb.Append("\"email\": \"" + email + "\",");
                    sb.Append("\"attributes\": {");
                    sb.Append("\"placa\":\"" + objResumen.Placa  + "\",");
                    sb.Append("\"kilom\":\"" + objResumen.Kilometraje + "\",");
                    sb.Append("\"acele\":\"" + objResumen.Aceleracion + "\",");
                    sb.Append("\"dista\":\"" + objResumen.Distancia + "\",");
                    sb.Append("\"velex\":\"" + objResumen.VelocidadExceso + "\",");
                    sb.Append("\"frena\":\"" + objResumen.Frenado + "\",");
                    sb.Append("\"hora\":\"" + objResumen.Horas + "\",");
                    sb.Append("\"traye\":\"" + objResumen.Trayectos + "\",");
                    sb.Append("\"velpr\":\"" + objResumen.VelocidadPromedio + "\",");
                    sb.Append("\"estbat\":\"" + objResumen.EstadoBateria + "\",");
                    sb.Append("\"img_bat\":\"" + objResumen.ImagenBateria + "\",");
                    sb.Append("\"asunto\":\"" + objResumen.AsuntoEmail  + "\",");
                    sb.Append("\"mes_anio\":\"" + objResumen.MesAnio + "\",");
                    sb.Append("\"dia\":\"" + objResumen.Dia + "\",");
                    sb.Append("\"event_items\": [");
                    sb.Append("]");
                    sb.Append("}");
                    sb.Append("}");

                    //sumar contador
                    contador++;

                    if (contador < correos.Length)
                    {
                        sb.Append(",");
                    }
                }

                sb.Append("]");
                sb.Append("}");

                streamWriter.Write(sb.ToString());
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                return result.ToString();
            }
        }

        public String enviarvariosmensual(String[] correos, String url, String token, String evento, List<ResumenEjecutivoUnoEN> arrResumen)
        {
            String AccessToken = token;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Headers.Add("Authorization", "" + AccessToken);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("{");
                sb.Append("\"eventName\": \"" + evento + "\",");
                sb.Append("\"items\": [");

                int contadorCorreo = 0;

                foreach (string correo in correos)
                {
                    string email = correo.Replace("$", "");
                    string mes_anio = "";
                    string dia = "";
                    string asunto = "";
                    foreach (ResumenEjecutivoUnoEN objResumen in arrResumen)
                    {
                        mes_anio = objResumen.MesAnio;
                        dia = objResumen.Dia;
                        asunto  = objResumen.AsuntoEmail;
                    }

                    sb.Append("{");
                    sb.Append("\"email\": \"" + email + "\",");
                    sb.Append("\"attributes\": {");
                    sb.Append("\"mes_anio\":\"" + mes_anio + "\",");
                    sb.Append("\"dia\":\"" + dia + "\",");
                    sb.Append("\"asunto\":\"" + asunto + "\",");
                    sb.Append("\"event_items\": [");

                    int contador = 0;

                    foreach (ResumenEjecutivoUnoEN objResumen in arrResumen)
                    {
                        sb.Append("{");
                        sb.Append("\"placa\":\"" + objResumen.Placa + "\",");
                        sb.Append("\"kilom\":\"" + objResumen.Kilometraje + "\",");
                        sb.Append("\"acele\":\"" + objResumen.Aceleracion + "\",");
                        sb.Append("\"dista\":\"" + objResumen.Distancia + "\",");
                        sb.Append("\"velex\":\"" + objResumen.VelocidadExceso + "\",");
                        sb.Append("\"frena\":\"" + objResumen.Frenado + "\",");
                        sb.Append("\"hora\":\"" + objResumen.Horas + "\",");
                        sb.Append("\"traye\":\"" + objResumen.Trayectos + "\",");
                        sb.Append("\"velpr\":\"" + objResumen.VelocidadPromedio + "\",");
                        sb.Append("\"estbat\":\"" + objResumen.EstadoBateria + "\",");
                        sb.Append("\"img_bat\":\"" + objResumen.ImagenBateria + "\",");
                        sb.Append("}");
                        contador++;

                        if (contador < arrResumen.Count)
                        {
                            sb.Append(",");
                        }

                    }
                    sb.Append("]");
                    sb.Append("}");
                    sb.Append("}");

                    contadorCorreo++;

                    if (contadorCorreo < correos.Length)
                    {
                        sb.Append(",");
                    }
                    //sumar contador
                }

                sb.Append("]");
                sb.Append("}");

                streamWriter.Write(sb.ToString());
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result.ToString();
            }


        }
    }
}
