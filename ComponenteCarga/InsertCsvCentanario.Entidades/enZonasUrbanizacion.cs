using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace InsertCsvCentanario.Entidades
{
    public class enZonasUrbanizacion
    {
        public const string NombredeLista = "ZonasUrbanizacion";

        public const string SP_Titulo = "Title";
        public const string SP_Supervisor = "Supervisor";


        public FieldLookupValue Supervisor { get; set; }
        public int Zona { get; set; }

    }
}
