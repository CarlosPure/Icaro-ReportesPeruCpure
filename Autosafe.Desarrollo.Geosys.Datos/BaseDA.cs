using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autosafe.Desarrollo.Geosys.Datos
{
    public class BaseDA
    {
        protected internal SqlConnection Conectar()
        {
            
            return new SqlConnection(ConfigurationManager.ConnectionStrings["GEOSYSConnectionStringMain"].ToString());
        }

        protected internal SqlConnection Conectar2(int id)
        {
            if (id == 217)
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["Icaro.My.MySettings.GEOSYSConnectionStringMain"].ToString());
            }
            else if (id == 25)
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["Icaro.My.MySettings.GEOSYSConnectionStringAux"].ToString());
            }
            else
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["Icaro.My.MySettings.GEOSYSConnectionStringMain"].ToString());
            }
        }

        
    }
}
