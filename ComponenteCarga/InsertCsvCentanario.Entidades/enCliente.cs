using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertCsvCentanario.Entidades
{
    public class enCliente : enCobranza
    {
        public const string SP_Cli_RazonSocial = "Title";
        public const string SP_Cli_CodigoCliente = "Codigo";
        public const string SP_Cli_Ejecutivo = "Ejecutivo";
        public const string SP_Cli_Division = "Division";
        public const string SP_Cli_Sociedad = "Sociedad";

        public int IdCliente { get; set; }
        public string RazonSocial { get; set; }
        public string CodigoCliente { get; set; }

        public string Action { get; set; }


    }



}
