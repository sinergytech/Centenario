using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daPoryectos
    {
        public List<enProyectos> ListarProyectos(SP.ClientContext contexto)
        {
            enProyectos proyecto = new enProyectos();
            List<enProyectos> lstproyectos = new List<enProyectos>();
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
            <Query>
            </Query>
            </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.Proyectos);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count > 0)
            {
                //SP.ListItem item = registros.FirstOrDefault();

                foreach (SP.ListItem item in registros)
                {
                    proyecto = new enProyectos();
                    //ejecutivosUrbanizacion.Ejecutivo = ((SP.FieldLookupValue)item["Ejecutivo"]).LookupValue;
                    proyecto.Id = item.Id;
                    proyecto.Nombre=item["Title"].ToString();
                    if (item["Codigo"] != null)
                    {
                        proyecto.Codigo = item["Codigo"].ToString();
                    }
                    lstproyectos.Add(proyecto);

                }
                //return ((SP.FieldLookupValue)item["Ejecutivo"]).LookupValue;
            }
            else
            {
                lstproyectos = new List<enProyectos>();
            }

            return lstproyectos;
        }
    }
}
