using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertCsvCentenario.AccesoDatos
{
    public class Utils
    {

        public const string COBRANZA_CC01 = "CC01";
        public const string PORCOBRAR_CC03 = "CC03";
        public const string PORCOBRAR_C002 = "CC02";


        public const string URBANIZACION = "DU01";
        public const string INDUSTRIAL = "DU02";
        
        public const string MINKA_RI03 = "RI03";


        public string[] leerCsv(string ruta)
        {
            string[] registros = null;

            if (File.Exists(ruta))
            {
                registros = File.ReadAllLines(ruta, Encoding.UTF8);
            }

            return registros;

        }

        public bool ValidarSiExisteRutaCarpeta(string Ruta)
        {

            bool rpta;
            if (Directory.Exists(Ruta))
            {
                rpta = true;
            }
            else
            {

                rpta = false;
            }
            return rpta;

        }


    }
}
