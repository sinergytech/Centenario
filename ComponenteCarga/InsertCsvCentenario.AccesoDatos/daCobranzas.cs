using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;
using System.Globalization;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daCobranzas
    {
        public string InsertarCobranzas(enCliente cli, SP.ClientContext contexto)
        {
            try
            {

                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enCobranza.NombreListaSPconbranzas);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);


                SP.FieldLookupValue LookUpCliente = new SP.FieldLookupValue() { LookupId = cli.IdCliente };

                RegistroLista[enCobranza.SP_Cob_Cliente] = LookUpCliente;
                RegistroLista[enCobranza.SP_Cob_Anio] = cli.Anio;
                RegistroLista[enCobranza.SP_Cob_Mes] = cli.Mes;
                RegistroLista[enCobranza.SP_Cob_Monto] = cli.Venta;
                RegistroLista[enCobranza.SP_Cob_Division] = cli.Division;

                RegistroLista.Update();
                contexto.ExecuteQuery();

                return "Cobranza registrada: " + cli.Venta;

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }
        public string ActualizarMonto_x_Cliente(enCliente cli, SP.ClientContext contexto)
        {



            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                  <And>     
                    <And>     
                      <And>     
                          <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Anio + @"'/>  <Value Type='Number'>" + cli.Anio + @"</Value> </Eq>
                          <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Mes + @"'/>  <Value Type='Number'>" + cli.Mes + @"</Value> </Eq>
                      </And> 
                          <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Cliente + @"'/>  <Value Type='Text'>" + cli.RazonSocial + @"</Value> </Eq>
                  </And>     
                          <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Division + @"'/>  <Value Type='Text'>" + cli.Division + @"</Value> </Eq>
                  </And> 
                       </Where>
                      </Query>
                     </View>";

            try
            {
                SP.List oList = contexto.Web.Lists.GetByTitle(enCobranza.NombreListaSPconbranzas);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                SP.ListItem item = registros.FirstOrDefault();

                decimal total = Convert.ToDecimal(item[enCobranza.SP_Cob_Monto], new CultureInfo("en-ES")) + Convert.ToDecimal(cli.Venta, new CultureInfo("en-ES"));
                //decimal VentaTotal = total + cli.Venta;
                item[enCobranza.SP_Cob_Monto] = total;

                item.Update();
                contexto.ExecuteQuery();


                return "Cobranza del Cliente : " + cli.RazonSocial + " actualizada";

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }
        public bool ListarCobranzaPorClienteMesAnio(enCliente cli, SP.ClientContext contexto)
        {


            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();

            //query.ViewXml = @"<View>
            //           <Query>
            //            <Where>
            //      <And>  
            //        <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Anio + @"'/>  <Value Type='Number'>" + cli.Anio + @"</Value> </Eq>
            //        <And> 
            //            <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Cliente + @"'/>  <Value Type='Text'>" + cli.RazonSocial + @"</Value> </Eq>
            //          <And>     
            //              <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Division + @"'/>  <Value Type='Text'>" + cli.Division + @"</Value> </Eq>
            //              <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Mes + @"'/>  <Value Type='Number'>" + cli.Mes + @"</Value> </Eq>
            //          </And> 

            //      </And>     

            //      </And> 
            //           </Where>
            //          </Query>
            //         </View>";

            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                    <Eq> @<FieldRef Name='" + enCobranza.SP_Cob_Cliente + @"'/>  <Value Type='Text'>" + cli.RazonSocial + @"</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = web.Lists.GetByTitle(enCobranza.NombreListaSPconbranzas);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();

            if (registros.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }


}
