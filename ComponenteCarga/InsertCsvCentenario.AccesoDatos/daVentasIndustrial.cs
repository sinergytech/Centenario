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
    public class daVentaIndusrial
    {
        public string InsertarRegistro(enVentasIndustrial VenIndus, SP.ClientContext contexto)
        {
            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                // enVentasIndustrial EjecutivoLookUp = RecuperarEjecuctivoApoyo_y_Ejecutivo(VenIndus.CodigoAsesor);

                RegistroLista[enVentasIndustrial.SP_Zona] = VenIndus.Zona;
                RegistroLista[enVentasIndustrial.SP_Ejecutivo] = VenIndus.LookUpEjecutivo;
                RegistroLista[enVentasIndustrial.SP_VentaTotal] = VenIndus.Venta;
                RegistroLista[enVentasIndustrial.SP_Anio] = VenIndus.Anio;
                RegistroLista[enVentasIndustrial.SP_Mes] = VenIndus.Mes;
                RegistroLista[enVentasIndustrial.SP_TipoVenta] = VenIndus.TipoVenta;
                RegistroLista[enVentasIndustrial.SP_EjecutivoApoyo] = VenIndus.LookUpEjecutivoApoyo;
                RegistroLista[enVentasIndustrial.SP_Supervisor] = VenIndus.SupervisorLookUp;
                RegistroLista[enVentasIndustrial.SP_TipoLote] = VenIndus.TipoLote;
                RegistroLista[enVentasIndustrial.SP_Area] = VenIndus.Area;
                RegistroLista[enVentasIndustrial.SP_Rol] = VenIndus.RolLookUp;
                RegistroLista[enVentasIndustrial.SP_AnioResolucion] = VenIndus.AnioResolucion;
                RegistroLista[enVentasIndustrial.SP_MesResolucion] = VenIndus.MesResolucion;
                RegistroLista[enVentasIndustrial.SP_Anio_Arras] = VenIndus.AnioArras;
                RegistroLista[enVentasIndustrial.SP_Mes_Arras] = VenIndus.MesArras;

                RegistroLista.Update();
                contexto.ExecuteQuery();

                return "Cobranza registrada: " + VenIndus.Venta;
            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }

        public bool ValidarEjecutivoAnioMes(enVentasIndustrial VenIndus, string NombreEjecutivo, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + NombreEjecutivo + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VenIndus.Anio + @"</Value> </Eq>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VenIndus.Mes + @"</Value> </Eq>
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
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


        public enVentasIndustrial RecuperarEjecuctivoApoyo_y_Ejecutivo(string CodEjecutivo, SP.ClientContext contexto)
        {
            enVentasIndustrial EjecutivoIndustrial = new enVentasIndustrial();

            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>      
                          <Eq> @<FieldRef Name='Rol'/>  <Value Type='Text'>Asesor Industrial</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = web.Lists.GetByTitle("Ejecutivos Industrial");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            foreach (var item in registros)
            {
                if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null && item["CodigoAsesor"].ToString() == CodEjecutivo)
                {
                    EjecutivoIndustrial.LookUpEjecutivo = (SP.FieldUserValue)item["Ejecutivo"];
                }
                if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null && item["CodigoAsesor"].ToString() != CodEjecutivo)
                {
                    EjecutivoIndustrial.LookUpEjecutivoApoyo = (SP.FieldUserValue)item["Ejecutivo"];
                }

            }

            return EjecutivoIndustrial;


        }

        public string ActualizarEjecitivoAnioMes(enVentasIndustrial VentasIndu, SP.ClientContext contexto)
        {
            string Rpta = string.Empty;
            try
            {
                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VentasIndu.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VentasIndu.Anio + @"</Value> </Eq>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VentasIndu.Mes + @"</Value> </Eq>
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";


                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    decimal total = Convert.ToDecimal(item[enVentasUrbanizacion.SP_VentaTotal], new CultureInfo("en-ES")) + Convert.ToDecimal(VentasIndu.Venta, new CultureInfo("en-ES"));
                    //decimal VentaTotal = total + VentasIndu.Venta;
                    item[enVentasUrbanizacion.SP_VentaTotal] = total;

                    item.Update();
                    contexto.ExecuteQuery();

                    Rpta = "Venta industrial: " + VentasIndu.LookUpEjecutivo.LookupValue + " actualizada - Venta Total: " + total;
                }

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }


        #region Version2


        public bool ValidarExisteEjecutivoIndustrial(enVentasIndustrial VenIndus, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VenIndus.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VenIndus.Anio + @"</Value> </Eq>
                                    <And>
                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VenIndus.Mes + @"</Value> </Eq>
                                            <And>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoLote + "'/>  <Value Type='Text'>" + VenIndus.TipoLote + @"</Value> </Eq>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoVenta + "'/>  <Value Type='Text'>" + VenIndus.TipoVenta + @"</Value> </Eq>
                                           </And>
                                   </And> 
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public List<enVentasIndustrial> ListarEjecutivoIndustrial(SP.ClientContext contexto)
        {
            enVentasIndustrial EjecutivoIndustrial = new enVentasIndustrial();
            List<enVentasIndustrial> lstEjecutivoIndustrial = new List<enVentasIndustrial>();
            //SP.ClientContext contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();
            SP.Web web = contexto.Web;

            contexto.Load(web);
            contexto.ExecuteQuery();

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
            <Query>
            </Query>
            </View>";

            SP.List oList = web.Lists.GetByTitle("Ejecutivos Industrial");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            foreach (var item in registros)
            {
                EjecutivoIndustrial = new enVentasIndustrial();
                if (item["CodigoAsesor"] != null)
                {
                    EjecutivoIndustrial.CodigoAsesor = item["CodigoAsesor"].ToString();
                    if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.LookUpEjecutivo = (SP.FieldUserValue)item["Ejecutivo"];
                    }
                    if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.LookUpEjecutivoApoyo = (SP.FieldUserValue)item["Ejecutivo"];
                    }
                    if (item["Supervisor"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.SupervisorLookUp = (SP.FieldUserValue)item["Supervisor"];
                    }
                    if (item["Rol"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.RolLookUp = (SP.FieldLookupValue)item["Rol"];
                    }
                    lstEjecutivoIndustrial.Add(EjecutivoIndustrial);
                }

            }

            return lstEjecutivoIndustrial;
        }

        #endregion

    }



}
