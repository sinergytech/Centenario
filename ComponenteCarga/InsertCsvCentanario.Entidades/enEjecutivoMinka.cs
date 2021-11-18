using SP = Microsoft.SharePoint.Client;

namespace InsertCsvCentanario.Entidades
{
    public class enEjecutivoMinka
    {
        //Nombre de la lista
        public const string NombreListaSP = "Contratos Minka";
        public const string Proyectos = "Proyectos";


        //Columnas de la lista
     
        public const string SP_Contrato = "Contrato";
        public const string SP_Ejecutivo = "Ejecutivo";
       
        public const string SP_Anio = "Anio";
        public const string SP_Mes = "Mes";
        public const string SP_Monto = "Monto";
        public const string SP_Title = "Title";

        public const string SP_CodigoProyecto = "Codigo";
        public const string SP_IdProyecto = "Proyecto";
        public const string SP_Supervisor = "Supervisor";
        public const string SP_Rol = "RolAsesor";



        public string Contrato { get; set; }
        public string Anio { set; get; }
        public string Mes { set; get; }
        public decimal Monto { set; get; }

        
        public SP.FieldUserValue EjecutivoLookUp { get; set; }
    }
}
