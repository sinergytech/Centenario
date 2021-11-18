using InsertCsvCentanario.Entidades;
using SP = Microsoft.SharePoint.Client;
using System.Globalization;
using System;
using System.Linq;
namespace InsertCsvCentenario.AccesoDatos
{
    public class daRentaInmobiliaria
    {
       // public static SP.ClientContext Contexto = new cnxSharePoint().ObtenerRentaInmobiliaria();
        public bool ValidarExisteContratMinka(enEjecutivoMinka ejecutivoMinka , SP.ClientContext Contexto)
        {
            // SP.ClientContext contexto = Contexto;
            try
            {
                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>
                              <And>
                                 <Eq>
                                    <FieldRef Name='Contrato' />
                                    <Value Type='Text'>" + ejecutivoMinka.Contrato + @"</Value>
                                 </Eq>
                                 <And>
                                    <Eq>
                                       <FieldRef Name='Anio' />
                                       <Value Type='Number'>" + ejecutivoMinka.Anio + @"</Value>
                                    </Eq>
                                    <Eq>
                                       <FieldRef Name='Mes' />
                                       <Value Type='Number'>" + ejecutivoMinka.Mes + @"</Value>
                                    </Eq>
                                 </And>
                              </And>
                           </Where>
                     </View>";


                SP.List oList = Contexto.Web.Lists.GetByTitle(enEjecutivoMinka.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                Contexto.Load(registros);
                Contexto.ExecuteQuery();


                if (registros.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
          



        }


        public string InsertarContratMinka(enEjecutivoMinka ejecutivoMinka, SP.ClientContext Contexto)
        {

            // SP.ClientContext Contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();

            try
            {
                SP.List ObjetoLista = Contexto.Web.Lists.GetByTitle(enEjecutivoMinka.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);
                //enVentasUrbanizacion ZonaYEjecutivo = RecuperarZona_y_Ejecutivo(VentasUrba.CodigoAsesor);
                //RegistroLista[enEjecutivoMinka.SP_Ejecutivo] = ejecutivoMinka.EjecutivoLookUp;
                RegistroLista[enEjecutivoMinka.SP_Contrato] = ejecutivoMinka.Contrato;
                RegistroLista[enEjecutivoMinka.SP_Monto] = ejecutivoMinka.Monto;
                RegistroLista[enEjecutivoMinka.SP_Anio] = ejecutivoMinka.Anio;
                RegistroLista[enEjecutivoMinka.SP_Mes] = ejecutivoMinka.Mes;



                RegistroLista.Update();
                Contexto.ExecuteQuery();

                //return "Venta urbanizacion registrada: " + ejecutivoMinka.EjecutivoLookUp.LookupValue + " - " + ejecutivoMinka.Monto;
                return "Venta urbanizacion registrada:- " + ejecutivoMinka.Monto;
            }
            catch (Exception ex)
            {
                return "Error es " + ex.Message;
            }

        }

        public string ActualizarContratMinka(enEjecutivoMinka ejecutivoMinka, SP.ClientContext Contexto)
        {
            string Rpta = string.Empty;
            try
            {
                // SP.ClientContext contexto = Contexto;

                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>
                              <And>
                                 <Eq>
                                    <FieldRef Name='Contrato' />
                                    <Value Type='Text'>" + ejecutivoMinka.Contrato + @"</Value>
                                 </Eq>
                                 <And>
                                    <Eq>
                                       <FieldRef Name='Anio' />
                                       <Value Type='Number'>" + ejecutivoMinka.Anio + @"</Value>
                                    </Eq>
                                    <Eq>
                                       <FieldRef Name='Mes' />
                                       <Value Type='Number'>" + ejecutivoMinka.Mes + @"</Value>
                                    </Eq>
                                 </And>
                              </And>
                           </Where>
                     </View>";


                SP.List oList = Contexto.Web.Lists.GetByTitle(enEjecutivoMinka.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                Contexto.Load(registros);
                Contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    decimal total = Convert.ToDecimal(item[enEjecutivoMinka.SP_Monto], new CultureInfo("en-ES")) + Convert.ToDecimal(ejecutivoMinka.Monto, new CultureInfo("en-ES"));
                    item[enEjecutivoMinka.SP_Monto] = total;

                    item.Update();
                    Contexto.ExecuteQuery();

                    Rpta = "Venta urbanizacion: " + ejecutivoMinka.EjecutivoLookUp.LookupValue + " actualizada - Venta Total: " + total;
                }

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }
    }
}
