using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daCliente
    {

        public List<enCliente> ListarClientesConDetalle(string codigo, string Division, SP.ClientContext contexto)
        {
            List<enCliente> lstClientes = new List<enCliente>();


            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                           <And>
                          <Eq> @<FieldRef Name='Codigo'/>  <Value Type='Text'>" + codigo + @"</Value> </Eq>
                          <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + Division + @"</Value> </Eq>
                         </And>            
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);


            contexto.Load(registros);
            contexto.ExecuteQuery();

            enCliente cli = null;
            foreach (SP.ListItem item in registros)
            {
                cli = new enCliente();

                cli.CodigoCliente = string.IsNullOrEmpty(item[enCliente.SP_Cli_CodigoCliente].ToString()) ? string.Empty : item[enCliente.SP_Cli_CodigoCliente].ToString();
                cli.RazonSocial = string.IsNullOrEmpty(item[enCliente.SP_Cli_RazonSocial].ToString()) ? string.Empty : item[enCliente.SP_Cli_RazonSocial].ToString();


                if (!string.IsNullOrEmpty(item[enCliente.SP_Cli_Ejecutivo].ToString()))
                {
                    SP.FieldUserValue userValue = (SP.FieldUserValue)item[enCliente.SP_Cli_Ejecutivo];
                    cli.Ejecutivo = userValue.LookupValue;
                }


                cli.Division = string.IsNullOrEmpty(item[enCliente.SP_Cli_Division].ToString()) ? string.Empty : item[enCliente.SP_Cli_Division].ToString();

                lstClientes.Add(cli);
            }


            return lstClientes;
        }

        public int ListarClienteRetornaID(enCliente cli, SP.ClientContext contexto)
        {
            int Id = 0;


            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                        <And>
                          <Eq> @<FieldRef Name='Codigo'/>  <Value Type='Text'>" + cli.CodigoCliente + @"</Value> </Eq>
                            <And>
                            <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + cli.Division + @"</Value> </Eq>
                            <Eq> @<FieldRef Name='Sociedad'/>  <Value Type='Text'>" + cli.Sociedad + @"</Value> </Eq>
                            </And>            
                        </And>            
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);


            contexto.Load(registros);
            contexto.ExecuteQuery();

            if (registros.Count > 0)
            {
                SP.ListItem item = registros.FirstOrDefault();
                Id = item.Id;
            }

            return Id;
        }

        public int ActualizarSociedad(enCliente cli, SP.ClientContext contexto)
        {
            int Id = 0;


            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();


            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                        <And>
                          <Eq> @<FieldRef Name='Codigo'/>  <Value Type='Text'>" + cli.CodigoCliente + @"</Value> </Eq>
                          <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + cli.Division + @"</Value> </Eq>
                        </And>            
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);


            contexto.Load(registros);
            contexto.ExecuteQuery();

            if (registros.Count > 0)
            {
                SP.ListItem item = registros.FirstOrDefault();
                item["Sociedad"] = cli.Sociedad;

                item.Update();
                contexto.ExecuteQuery();
                Id = item.Id;
            }

            return Id;
        }

        public string ActualizarCliente(enCliente cli, SP.ClientContext contexto)
        {
            try
            {
                SP.List oList = contexto.Web.Lists.GetByTitle("Clientes");
                SP.ListItem item = oList.GetItemById(cli.IdCliente);

                contexto.Load(item);
                contexto.ExecuteQuery();


                item["Sociedad"] = cli.Sociedad;

                item.Update();
                contexto.ExecuteQuery();


                return "Cobranza del Cliente : " + cli.RazonSocial + " actualizada";

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }

        public SP.ListItemCollection ListarClienteDivision(string division, SP.ClientContext contexto)
        {
            int Id = 0;


            SP.Web web = contexto.Web;
            //contexto.Load(web);
            //contexto.ExecuteQuery();
            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                            <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + division + @"</Value> </Eq>
                       </Where>
                      </Query>
                        <RowLimit>100</RowLimit>
                     </View>";

            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();

            //if (registros.Count > 0)
            //{
            //    SP.ListItem item = registros.FirstOrDefault();
            //    Id = item.Id;
            //}

            return registros;
        }

        public SP.ListItemCollection ListarClientePorSociedad(string Sociedad, SP.ClientContext contexto)
        {
            int Id = 0;


            SP.Web web = contexto.Web;
            //contexto.Load(web);
            //contexto.ExecuteQuery();
            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                       <Query>
                        <Where>
                            <And>
                            <Eq> @<FieldRef Name='Sociedad'/>  <Value Type='Text'>" + Sociedad + @"</Value> </Eq>
                            <IsNull><FieldRef Name='Ejecutivo' /></IsNull>
                            </And>
                       </Where>
                            <OrderBy> 
                                <FieldRef Name='ID'></FieldRef> 
                            </OrderBy> 
                      </Query>
                        <RowLimit>5000</RowLimit>
                     </View>";

            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();

            //if (registros.Count > 0)
            //{
            //    SP.ListItem item = registros.FirstOrDefault();
            //    Id = item.Id;
            //}

            return registros;
        }

        public SP.ListItemCollection ListarClientePorSociedadYUnidad(string Sociedad, string division, SP.ClientContext contexto)
        {
            int Id = 0;


            SP.Web web = contexto.Web;
            //contexto.Load(web);
            //contexto.ExecuteQuery();
            SP.CamlQuery query = new SP.CamlQuery();
            query.ViewXml = @"<View>
                        <ViewFields>
                                <FieldRef Name='Title'/>
                            </ViewFields>
                       <Query>
                        <Where>
                            <And>
                            <Eq> @<FieldRef Name='Sociedad'/>  <Value Type='Text'>" + Sociedad + @"</Value> </Eq>
                                <And>
                                <Eq> @<FieldRef Name='Division'/>  <Value Type='Text'>" + division + @"</Value> </Eq>
                                <IsNull><FieldRef Name='Ejecutivo' /></IsNull>
                                </And>
                            </And>
                       </Where>
                            <OrderBy> 
                                <FieldRef Name='Sociedad'></FieldRef> 
                            </OrderBy> 
                      </Query>
                        <RowLimit>5000</RowLimit>
                     </View>";

            SP.List oList = web.Lists.GetByTitle("Clientes");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();

            //if (registros.Count > 0)
            //{
            //    SP.ListItem item = registros.FirstOrDefault();
            //    Id = item.Id;
            //}

            return registros;
        }

        public string InsertarRegistro_y_Cobranza(enCliente cli, SP.ClientContext contexto)
        {
            try
            {

                /*Primer insert [Clientes]*/
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle("Clientes");
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                RegistroLista[enCliente.SP_Cli_RazonSocial] = cli.RazonSocial;
                RegistroLista[enCliente.SP_Cli_CodigoCliente] = cli.CodigoCliente;
                RegistroLista[enCliente.SP_Cli_Division] = cli.Division;
                RegistroLista[enCliente.SP_Cli_Sociedad] = cli.Sociedad;
                RegistroLista.Update();
                contexto.ExecuteQuery();

                /*Segundo Insert  [Cobranzas]*/
                cli.IdCliente = RegistroLista.Id;
                string rptaInsert = new daCobranzas().InsertarCobranzas(cli, contexto);


                return "Codigo Cliente: " + cli.CodigoCliente + " y R. social: " + cli.RazonSocial + " " + rptaInsert;

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }

        public string InsertarRegistro_y_CuentasXCobrar(enCliente cli, SP.ClientContext contexto)
        {



            try
            {

                /*Primer insert [Clientes]*/
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle("Clientes");
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                RegistroLista[enCliente.SP_Cli_RazonSocial] = cli.RazonSocial;
                RegistroLista[enCliente.SP_Cli_CodigoCliente] = cli.CodigoCliente;
                RegistroLista[enCliente.SP_Cli_Division] = cli.Division;
                RegistroLista[enCliente.SP_Cli_Sociedad] = cli.Sociedad;

                RegistroLista.Update();
                contexto.ExecuteQuery();
                /*Segundo Insert  [CuentasXCobrar]*/
                cli.IdCliente = RegistroLista.Id;
                string rptaInsert = new daCuentasXCobrar().InsertarCuentaPorCobrar(cli, contexto);

                return "Codigo Cliente: " + cli.CodigoCliente + " y R. social: " + cli.RazonSocial + " " + rptaInsert;

            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }


        public int InsertarCliente_y_CuentasXCobrar(enCliente cli, SP.ClientContext contexto)
        {
            int Id = 0;
            try
            {

                /*Primer insert [Clientes]*/
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle("Clientes");
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                RegistroLista[enCliente.SP_Cli_RazonSocial] = cli.RazonSocial;
                RegistroLista[enCliente.SP_Cli_CodigoCliente] = cli.CodigoCliente;
                RegistroLista[enCliente.SP_Cli_Division] = cli.Division;


                RegistroLista.Update();
                contexto.ExecuteQuery();
                Id = RegistroLista.Id;
                /*Segundo Insert  [CuentasXCobrar]*/
                // string rptaInsert = new daCuentasXCobrar().InsertarCuentaPorCobrar(cli, RegistroLista.Id);


                // return "Codigo Cliente: " + cli.CodigoCliente + " y R. social: " + cli.RazonSocial + " " + rptaInsert;
                return Id;
            }
            catch (Exception ex)
            {

                return Id;
            }

        }


    }
}
