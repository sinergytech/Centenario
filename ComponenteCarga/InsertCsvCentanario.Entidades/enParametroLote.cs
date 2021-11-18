using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertCsvCentanario.Entidades
{
    public class enParametroLote
    {
        public const string SP_AreaDesde = "AreaDesde";
        public const string SP_AreaHasta = "AreaHasta";
        public const string SP_TipoVenta = "TipoVenta";
        public const string SP_TipoLote = "TipoLote";
        public decimal AreaDesde { get; set; }
        public decimal AreaHasta { get; set; }
        public string TipoVenta { get; set; }
        public string TipoLote { get; set; }
    }
}
