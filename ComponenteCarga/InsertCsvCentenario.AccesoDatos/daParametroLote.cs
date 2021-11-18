using InsertCsvCentanario.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
namespace InsertCsvCentenario.AccesoDatos
{
    public class daParametroLote
    {
        public enParametroLote ObtenerParametroLote(decimal AreaTerreno, SP.ClientContext contexto)
        {
            SP.Web web = contexto.Web;

            contexto.Load(web);
            contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                                <Query>
                                   <Where>
                                      <And>
                                         <Lt>
                                            <FieldRef Name='AreaDesde' />
                                            <Value Type='Number'>" + AreaTerreno + @"</Value>
                                         </Lt>
                                         <Geq>
                                            <FieldRef Name='AreaHasta' />
                                            <Value Type='Number'>" + AreaTerreno + @"</Value>
                                         </Geq>
                                      </And>
                                   </Where>
                                </Query>
                            </View>";

            //query.ViewXml = @"<View>
            //           <Query>
            //            <Where>
            //      <And>
            //              <Eq> @<FieldRef Name='Codigo'/>  <Value Type='Text'>" + codigo + @"</Value> </Eq>
            //              <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + Division + @"</Value> </Eq>
            //      </And>            
            //           </Where>
            //          </Query>
            //         </View>";


            SP.List oList = web.Lists.GetByTitle("Parámetros Lotes");
            SP.ListItemCollection registros = oList.GetItems(query);


            contexto.Load(registros);
            contexto.ExecuteQuery();

            enParametroLote parametroLote = null;
            foreach (SP.ListItem item in registros)
            {
                parametroLote = new enParametroLote();

                parametroLote.AreaDesde = string.IsNullOrEmpty(item[enParametroLote.SP_AreaDesde].ToString()) ? 0 : Convert.ToDecimal( item[enParametroLote.SP_AreaDesde]);
                parametroLote.AreaHasta = string.IsNullOrEmpty(item[enParametroLote.SP_AreaHasta].ToString()) ? 0 : Convert.ToDecimal(item[enParametroLote.SP_AreaHasta]);
                parametroLote.TipoVenta = string.IsNullOrEmpty(item[enParametroLote.SP_TipoVenta].ToString()) ? string.Empty : item[enParametroLote.SP_TipoVenta].ToString();
                parametroLote.TipoLote = string.IsNullOrEmpty(item[enParametroLote.SP_TipoLote].ToString()) ? string.Empty : item[enParametroLote.SP_TipoLote].ToString();
            }


            return parametroLote;
        }

        public List<enParametroLote> ObtenerParametrosLote(SP.ClientContext contexto)
        {
            SP.Web web = contexto.Web;

            SP.CamlQuery query = new SP.CamlQuery();
            
            SP.List oList = web.Lists.GetByTitle("Parámetros Lotes");
            SP.ListItemCollection registros = oList.GetItems(SP.CamlQuery.CreateAllItemsQuery());


            contexto.Load(registros);
            contexto.ExecuteQuery();

            List<enParametroLote> lstParametroLote = new List<enParametroLote>();
            foreach (SP.ListItem item in registros)
            {
                enParametroLote parametroLote = new enParametroLote();

                parametroLote.AreaDesde = string.IsNullOrEmpty(item[enParametroLote.SP_AreaDesde].ToString()) ? 0 : Convert.ToDecimal(item[enParametroLote.SP_AreaDesde]);
                parametroLote.AreaHasta = string.IsNullOrEmpty(item[enParametroLote.SP_AreaHasta].ToString()) ? 0 : Convert.ToDecimal(item[enParametroLote.SP_AreaHasta]);
                parametroLote.TipoVenta = string.IsNullOrEmpty(item[enParametroLote.SP_TipoVenta].ToString()) ? string.Empty : item[enParametroLote.SP_TipoVenta].ToString();
                parametroLote.TipoLote = string.IsNullOrEmpty(item[enParametroLote.SP_TipoLote].ToString()) ? string.Empty : item[enParametroLote.SP_TipoLote].ToString();
                lstParametroLote.Add(parametroLote);
            }


            return lstParametroLote;
        }
    }
}
