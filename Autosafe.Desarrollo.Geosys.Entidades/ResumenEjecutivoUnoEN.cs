using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Autosafe.Desarrollo.Geosys.Entidades
{
    public class ResumenEjecutivoUnoEN
    {
        public readonly string Table_Name = "ResumenEjecutivoUnoEN";

        
        public string Placa { get; set; }
        public string Kilometraje { get; set; }
        public string Aceleracion { get; set; }
        public string EstadoBateria { get; set; }
        public string Distancia { get; set; }
        public string VelocidadExceso { get; set; }
        public string Frenado { get; set; }
        public string Horas { get; set; }
        public string Trayectos { get; set; }
        public string VelocidadPromedio { get; set; }
        public string MesAnio { get; set; }
        public string Dia { get; set; }
        public string AsuntoEmail { get; set; }
        public string ImagenBateria { get; set; }

        public ResumenEjecutivoUnoEN() { }
        
    }
}
