using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertCsvCentanario.Entidades
{
    public class enUnidadCliente
    {

        public const string ListaNombreSP = "ClienteUnidad";

        public const string SP_Unidad = "kngg";
        public const string SP_CodigoCliente = "CodigoCliente";
        public const string SP_RazonSocial = "RazonSocial";
        public const string SP_Sociedad = "Sociedad";



        public string Unidad { set; get; }
        public string CodigoCliente { set; get; }
        public string RazonSocial { set; get; }
        public string Sociedad { set; get; }


    }
}
