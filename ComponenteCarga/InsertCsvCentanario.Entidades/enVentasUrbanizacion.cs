using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;

namespace InsertCsvCentanario.Entidades
{
    public class enVentasUrbanizacion
    {
        //Nombre de la lista
        public const string NombreListaSP = "Ventas Urbanizaciones";
        public const string Proyectos = "Proyectos";
  

        //Columnas de la lista
        public const string SP_Zona = "Zona";
        public const string SP_Ejecutivo = "Ejecutivo";
        public const string SP_VentaTotal = "VentaTotal";
        public const string SP_Anio = "Anio";
        public const string SP_Mes = "Mes";
        public const string SP_Title = "Title";
        public const string SP_CodigoProyecto = "Codigo";
        public const string SP_IdProyecto = "Proyecto";
        public const string SP_Supervisor = "Supervisor";
        public const string SP_Rol = "RolAsesor";
        public const string SP_Anio_Reso = "AnioResolucion";
        public const string SP_Mes_Reso = "MesResolucion";

       
        //Atributos de la clase
        public string Zona { set; get; }
        public string Ejecutivo { set; get; }
        public string VentaTotal { set; get; }
        public decimal Venta { set; get; }
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string CodigoAsesor { set; get; }
        public string CodigoProyecto { set; get; }
        public string Proyecto { set; get; }
        public int IdProyecto { get; set; }
        //Atributos para la recuperacion de datos
        public SP.FieldLookupValue ZonaLookUp { get; set; }
        public SP.FieldUserValue EjecutivoLookUp { get; set; }
        public SP.FieldUserValue SupervisorLookUp { get; set; }
        public SP.FieldLookupValue RolLookUp { get; set; }
        //Atributos para capturar el EjecutivoLookUp
        public SP.FieldUserValue LookUpEjecutivo { set; get; }
        public SP.FieldUserValue LookUpEjecutivoApoyo { set; get; }
        public string AnioResolucion { set; get; }
        public string MesResolucion { set; get; }
    }
}
