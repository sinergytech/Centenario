using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;

namespace InsertCsvCentanario.Entidades
{
    public class enVentasIndustrial
    {
        //Nombre de la lista
        public   const string NombreListaSP = "Ventas Industrial";

        //Columnas de la lista
        public const string SP_Zona = "Zona";
        public const string SP_Ejecutivo = "Ejecutivo";
        public const string SP_VentaTotal = "VentaTotal";
        public const string SP_Anio = "Anio";
        public const string SP_Mes = "Mes";
        public const string SP_TipoVenta = "TipoVenta";
        public const string SP_EjecutivoApoyo = "EjecutivoApoyo";
        public const string SP_Supervisor = "Supervisor";
        public const string SP_TipoLote = "TipoLote";
        public const string SP_Area = "Area";
        public const string SP_Rol = "RolAsesor";
        public const string SP_AnioResolucion = "AnioResolucion";
        public const string SP_MesResolucion = "MesResolucion";
        public const string SP_Anio_Arras = "AnioArras";
        public const string SP_Mes_Arras = "MesArras";
        public const string SP_NroFinanciamiento = "NumeroFinanciamiento";
        public const string SP_SupervisorAsignado = "SupervisorAsignado";
        public const string SP_SupervisorVenta = "SupervisorVenta";
        public const string SP_NumeroDeudor = "NumeroDeudor";
        public const string SP_DatosCliente = "DatosCliente";
        public const string SP_Activo = "Activo";


        public  string V000414 = "V000414";
        public  string V000425 = "V000425";

        //Atributos de clase
        public string Zona { set; get; }
        public string Ejecutivo { set; get; }
        public string VentaTotal { set; get; }
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string TipoVenta { set; get; }
        public string EjecutivoApoyo { set; get; }
        public string TipoLote { set; get; }
        public decimal Area { set; get; }
        public string CodigoAsesor { set; get; }

        public decimal Venta { get; set; }
        
                 
        //Atributos para capturar el EjecutivoLookUp

        public SP.FieldUserValue LookUpEjecutivo { set; get; }
        public SP.FieldUserValue LookUpEjecutivoApoyo { set; get; }
        public SP.FieldUserValue SupervisorLookUp { get; set; }
        public SP.FieldLookupValue RolLookUp { get; set; }
        public string AnioResolucion { set; get; }
        public string MesResolucion { set; get; }
        public string AnioArras { set; get; }
        public string MesArras { set; get; }
        public string NroFinanciamiento { set; get; }
        public string SupervisorAsignado { set; get; }
        public string SupervisorVenta { set; get; }
        public string NumeroDeudor { set; get; }
        public string DatosCliente { set; get; }
        
    }
}
