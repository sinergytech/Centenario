using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertCsvCentanario.Entidades
{
    public class enCobranza
    {

        public const  string NombreListaSPconbranzas = "Cobranzas";
        public const  string NombreListaSPcuentasXcobrar = "Cuentas por Cobrar";


        public const string SP_Cob_Cliente = "Cliente";
        public const string SP_Cob_Anio = "Anio";
        public const string SP_Cob_Mes = "Mes";
        public const string SP_Cob_Monto = "Monto";
        public const string SP_Cob_Division = "Division";
        public const string SP_Cob_Ejecutivo = "Ejecutivo";

        public const string SP_Cob_MontoPorCobrar = "MontoCuentasCobrar";
        public const string SP_Cob_MontoVencido = "MontoFacturasVencidas";
      

        public string Cliente { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public decimal Monto { get; set; }
        public decimal Venta { get; set; }
        public decimal MontoVencido { get; set; }
        public string Division { get; set; }
        public string Sociedad { get; set; }
        public string Ejecutivo { get; set; }

    }
}
