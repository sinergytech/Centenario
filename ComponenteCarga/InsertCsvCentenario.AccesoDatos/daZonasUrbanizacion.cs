using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InsertCsvCentanario.Entidades;

using SP = Microsoft.SharePoint.Client;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daZonasUrbanizacion
    {
        public enZonasUrbanizacion GetUrbanizacion(string Titulo)
        {
            enZonasUrbanizacion ZonaUrba = new enZonasUrbanizacion();

            SP.ClientContext contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();
           //SP.Web web = contexto.Web;
           //
           //contexto.Load(web);
           //contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>      
                          <Eq> @<FieldRef Name='Title'/>  <Value Type='Text'>" + Titulo + @"</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = contexto.Web.Lists.GetByTitle(enZonasUrbanizacion.NombredeLista);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count > 0)
            {
                SP.ListItem item = registros.FirstOrDefault();
                ZonaUrba.Zona =  item.Id;
                ZonaUrba.Supervisor =  (SP.FieldUserValue) item[enZonasUrbanizacion.SP_Supervisor];
            }

            return ZonaUrba;
        }

    }
}

