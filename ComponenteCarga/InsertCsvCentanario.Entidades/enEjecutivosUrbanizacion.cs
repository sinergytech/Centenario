using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
namespace InsertCsvCentanario.Entidades
{
    public class enEjecutivosUrbanizacion
    {

        public const string NombreListaSP = "Ejecutivos Urbanizaciones";

        public const string SP_CodigoAsesor = "CodigoAsesor";
        public const string SP_Ejecutivo = "Ejecutivo";
        public const string SP_Supervisor = "Supervisor";
        public const string SP_Rol = "Rol";
        public const string SP_Zona = "Zona";




        public string CodigoAsesor { get; set; }
        public string Ejecutivo { get; set; }
        public string Supervisor { get; set; }
        public string Rol { get; set; }
        public string Zona { get; set; }

        public SP.FieldLookupValue ZonaLookUp { get; set; }
        public SP.FieldUserValue EjecutivoLookUp { get; set; }
        public SP.FieldUserValue SupervisorLookUp { get; set; }
        public SP.FieldLookupValue RolLookUp { get; set; }



    }
}
