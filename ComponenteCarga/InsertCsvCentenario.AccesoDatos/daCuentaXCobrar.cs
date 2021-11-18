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
    public class daCuentasXCobrar
    {
       
        public string ActualizarMontoACero(SP.ClientContext contexto)
        {

            string Rpta = string.Empty;


           

            SP.CamlQuery query =  SP.CamlQuery.CreateAllItemsQuery();

            try
            {
                SP.List oList = contexto.Web.Lists.GetByTitle(enCobranza.NombreListaSPcuentasXcobrar);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                foreach (var item in registros)
                {
                    item[enCobranza.SP_Cob_MontoPorCobrar] = 0;
                    item[enCobranza.SP_Cob_MontoVencido] = 0;

                    item.Update();
                    contexto.ExecuteQuery();
                }

                 Rpta = "Cuentas por cobrar vencidas a 0.00" ;

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;
        }

        public string InsertarCuentaPorCobrar(enCliente cli, SP.ClientContext contexto)
        {

           

            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enCobranza.NombreListaSPcuentasXcobrar);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);


                SP.FieldLookupValue LookUpCliente = new SP.FieldLookupValue() { LookupId = cli.IdCliente };

                RegistroLista[enCobranza.SP_Cob_Cliente] = LookUpCliente;
                RegistroLista[enCobranza.SP_Cob_Anio] = Convert.ToDecimal(cli.Anio);
                RegistroLista[enCobranza.SP_Cob_Mes] = Convert.ToDecimal(cli.Mes);
                RegistroLista[enCobranza.SP_Cob_MontoPorCobrar] = cli.Monto;
                RegistroLista[enCobranza.SP_Cob_MontoVencido] = cli.MontoVencido;
                RegistroLista[enCobranza.SP_Cob_Division] = cli.Division;

                RegistroLista.Update();
                //contexto.ExecuteQuery();

                return "Cuenta por Cobrar registrada: " + cli.Monto;

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
                SP.List oList = contexto.Web.Lists.GetByTitle(enCobranza.NombreListaSPcuentasXcobrar);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                foreach (var item in registros)
                {
                    decimal totalCuentasCobrar = Convert.ToDecimal(item[enCobranza.SP_Cob_MontoPorCobrar], new CultureInfo("en-ES")) + Convert.ToDecimal(cli.Monto, new CultureInfo("en-ES"));
                    decimal totalFacturasVencidas = Convert.ToDecimal(item[enCobranza.SP_Cob_MontoVencido], new CultureInfo("en-ES")) + Convert.ToDecimal(cli.MontoVencido, new CultureInfo("en-ES"));

                    item[enCobranza.SP_Cob_MontoPorCobrar] = Convert.ToDecimal(cli.Monto, new CultureInfo("en-ES"));//totalCuentasCobrar;
                    item[enCobranza.SP_Cob_MontoVencido] = Convert.ToDecimal(cli.MontoVencido, new CultureInfo("en-ES"));//totalFacturasVencidas;

                    item.Update();
                    //contexto.ExecuteQuery();
                }

                return "Cuenta por Cobrar registrada: " + cli.RazonSocial + " actualizada";

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }

        public bool ListarAnioMesPorCobrar(enCliente cli, SP.ClientContext contexto)
        {

          
            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();


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





            SP.List oList = web.Lists.GetByTitle(enCobranza.NombreListaSPcuentasXcobrar);
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
